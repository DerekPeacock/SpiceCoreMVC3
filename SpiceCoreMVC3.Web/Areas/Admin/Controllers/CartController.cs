using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiceCoreMVC3.Web.Data;
using SpiceCoreMVC3.Web.Models;

namespace SpiceCoreMVC3.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public OrderHeader Order { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            OrderHeader order = GetShoppingCart();
            return View(order);
        }

        [Authorize]
        public OrderHeader GetShoppingCart()
        {
            OrderHeader order = null;

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null) // Customer is logged in
            {
                order = _db.OrderHeaders
                    .Where(c => c.UserId == claim.Value && c.OrderStatus == OrderStatus.STARTED)
                    .Include(o => o.OrderItems)
                    .FirstOrDefault();

                order.TotalCost = 0;

                foreach(var item in order.OrderItems)
                {
                    MenuItem menuItem = _db.MenuItems.Find(item.MenuItemId);
                    item.MenuItem = menuItem;
                    order.TotalCost += item.Quantity * item.Price;
                }

                var count = order.OrderItems.Count;
                HttpContext.Session.SetInt32(SpiceConstants.CART_COUNT, count);
            }

            return order;
        }
    }
}