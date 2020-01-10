using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Coupon> Coupons { get; set; }
    }
}
