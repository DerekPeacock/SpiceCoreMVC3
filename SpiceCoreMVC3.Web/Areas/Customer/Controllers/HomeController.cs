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

            if(claim != null)
            {
                var count = _context.Orders.Where(u => u.CustomerID == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SpiceConstants.CART_COUNT, count);
            }

            return View(homeVM);
        }

        public async Task<IActionResult> Details(int id)
        {
            //Order order = await CreateNewOrder(id);
            var menuItem = await _context.MenuItems.Include(m => m.SubCategory).Where(m => m.Id == id).FirstOrDefaultAsync();

            return View("MenuItem", menuItem);
        }

        private async Task<Order> CreateNewOrder(int id)
        {
            var menuItem = await _context.MenuItems.Include(m => m.SubCategory).Where(m => m.Id == id).FirstOrDefaultAsync();

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
        public async Task<IActionResult> Details([Bind("Id,Name,Description,Spicyness,ImageURL,CategoryId,SubCategoryId,Price")] MenuItem item)
        {
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userID = claim.Value;

                OrderHeader orderFromDb = await _context.OrderHeaders.Where(c => c.UserId == userID).FirstOrDefaultAsync();
                
                if(orderFromDb == null)
                {

                    OrderHeader order = new OrderHeader();
                    order.UserId = userID;

                    await _context.OrderHeaders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    OrderItem orderItem = new OrderItem()
                    {
                        Quantity = 1,
                        MenuItemId = item.Id,
                        OrderId = order.Id,
                        Price = item.Price
                    };

                    await _context.OrderItems.AddAsync(orderItem);
                    await _context.SaveChangesAsync();
                }

                //var count = _context.Orders.Where(c => c.CustomerID == order.CustomerID).ToList().Count();
                //HttpContext.Session.SetInt32(SpiceConstants.CART_COUNT, count);

                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }
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
