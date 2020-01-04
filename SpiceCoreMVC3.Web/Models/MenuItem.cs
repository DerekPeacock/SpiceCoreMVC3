using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Spicyness { get; set; }

        public enum SpicyRating { NA, NotSpicy, Spicy, VerySpicy, RedHot}

        [DataType(DataType.ImageUrl)]
        [StringLength(120)]
        public string ImageURL { get; set; }

        [StringLength(20)]
        public string CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }
    }
}
