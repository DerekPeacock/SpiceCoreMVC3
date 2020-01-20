using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpiceCoreMVC3.Web.Data;
using SpiceCoreMVC3.Web.Models;

namespace SpiceCoreMVC3.Web.Areas.Admin.Controllers
{
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
            Order = new OrderHeader();
            

            return View();
        }
    }
}