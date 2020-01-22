using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpiceCoreMVC3.Web.Data;
using SpiceCoreMVC3.Web.Models;
using SpiceCoreMVC3.Web.Models.ViewModels;

namespace SpiceCoreMVC3.Web.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        //parameter = ILogger<HomeController> logger

        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            //_logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel()
            {
                MenuItems = _context.MenuItems.ToList(),
                Categories = _context.Categories.ToList()
            };

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null) // Customer is logged in
            {
                OrderHeader order = _context.OrderHeaders
                    .Where(c => c.UserId == claim.Value && c.OrderStatus == OrderStatus.STARTED)
                    .Include(o => o.OrderItems)
                    .FirstOrDefault();

                var count = order.OrderItems.Count;
                HttpContext.Session.SetInt32(SpiceConstants.CART_COUNT, count);
            }

            return View(homeVM);
        }

        public async Task<IActionResult> Details(int id)
        {
            //Order order = await CreateNewOrder(id);
            var menuItem = await _context.MenuItems
                .Include(m => m.SubCategory).Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            return View("MenuItem", menuItem);
        }

        private async Task<Order> CreateNewOrder(int id)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.SubCategory)
                .Where(m => m.Id == id).FirstOrDefaultAsync();

            OrderHeader cart = new OrderHeader();

            //cart.OrderItems.Add(menuItem);
            Order order = new Order
            {
                MenuItem = menuItem,
                MenuItemID = menuItem.Id,
            };

            return order;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("Id,Name,Description,Spicyness,ImageURL,CategoryId,SubCategoryId,Price")] MenuItem menuItem)
        {
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userID = claim.Value;

                OrderHeader order = await _context.OrderHeaders
                    .Where(c => c.UserId == userID && c.OrderStatus == OrderStatus.STARTED)
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync();
                
                if(order == null)
                {

                    order = new OrderHeader();
                    order.UserId = userID;

                    await _context.OrderHeaders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    await AddOrderItem(menuItem, order);
                }
                else
                {
                    bool itemFound = false;

                    foreach(OrderItem orderItem in order.OrderItems)
                    {
                        if(orderItem.MenuItemId == menuItem.Id)
                        {
                            itemFound = true;
                            orderItem.Quantity++;

                            _context.Update(orderItem);
                            await _context.SaveChangesAsync();
                        }
                    }

                    if(itemFound == false)
                    {
                        await AddOrderItem(menuItem, order);
                    }
                }

                int count = order.OrderItems.Count;
                HttpContext.Session.SetInt32(SpiceConstants.CART_COUNT, count);

                return RedirectToAction("Index");
            }
            else
            {
                return View(menuItem);
            }
        }

        private async Task AddOrderItem(MenuItem menuItem, OrderHeader order)
        {
            OrderItem orderItem = new OrderItem()
            {
                Quantity = 1,
                MenuItemId = menuItem.Id,
                OrderId = order.Id,
                Price = menuItem.Price
            };

            order.OrderItems.Add(orderItem);

            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
