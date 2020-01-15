using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(20)]
        public string Name { get; set; }

        [Required, StringLength(10)]
        public string PostCode { get; set; }
    }
}
