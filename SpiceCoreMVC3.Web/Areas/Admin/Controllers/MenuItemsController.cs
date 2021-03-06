﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpiceCoreMVC3.Web.Data;
using SpiceCoreMVC3.Web.Models;
using SpiceCoreMVC3.Web.Models.ViewModels;

namespace SpiceCoreMVC3.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public MenuItemsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            
            MenuItemVM = new MenuItemViewModel()
            {
                MenuItem = new MenuItem()

            };
        }

        // GET: Admin/MenuItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MenuItems.Include(m => m.Category).Include(m => m.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: Admin/MenuItems/Create
        public IActionResult Create()
        {
            CreateDropDowns(null);

            return View();
        }

        // POST: Admin/MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Spicyness,ImageURL,CategoryId,SubCategoryId,Price")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuItem);
                await _context.SaveChangesAsync();

                //UploadImage(menuItem);

                return RedirectToAction(nameof(Index));
            }

            CreateDropDowns(menuItem);

            return View(menuItem);
        }

        /// <summary>
        /// This method checks wether any files have been selected for uploading
        /// If so the file is uploaded to the images folder, else the default food image
        /// is used.
        /// </summary>
        private void UploadImage(MenuItem menuItem)
        {
            string webRootpath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            // New image fil to upload
            if (files.Count() > 0)
            {
                if(menuItem.ImageURL != null)
                {
                    var oldImagePath = Path.Combine(webRootpath, menuItem.ImageURL.TrimStart('\\'));
                }

                var uploads = Path.Combine(webRootpath, "images");
                var extension = Path.GetExtension(files[0].FileName);
                string filename = "MenuItem" + menuItem.Id + extension;

                using (var filestream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                };

                menuItem.ImageURL = filename;
            }
            else // Use the Default Image
            {
                // Make a Copy of the file (Why not use the file??)
                //var uploads = Path.Combine(webRootpath, @"images\" + "DefaultFoodImage");
                //string filename = "MenuItem" + menuItem.Id + ".png";

                //System.IO.File.Copy(uploads, webRootpath + @"\images\" + filename);
                //menuItem.ImageURL = filename;
                menuItem.ImageURL = "DefaultFood.png";
            }

            _context.SaveChanges();
        }

        // GET: Admin/MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }

            CreateDropDowns(menuItem);

            return View(menuItem);
        }

        // POST: Admin/MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Spicyness,ImageURL,CategoryId,SubCategoryId,Price")] MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuItem);
                    await _context.SaveChangesAsync();

                    //UploadImage(menuItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            CreateDropDowns(menuItem);

            return View(menuItem);
        }

        // GET: Admin/MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .Include(m => m.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: Admin/MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }

        private void CreateDropDowns(MenuItem menuItem)
        {
            if(menuItem == null)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
                ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "CategoryId");
            }
            else
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", menuItem.CategoryId);
                ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name", menuItem.SubCategoryId);
            }
        }
    }
}
