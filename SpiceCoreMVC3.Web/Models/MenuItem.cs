using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText), StringLength(1000)]
        public string Description { get; set; }

        public SpicyRating Spicyness { get; set; }

        [DataType(DataType.ImageUrl)]
        [StringLength(120), Display(Name ="Image Upload")]
        public string ImageURL { get; set; }

        [StringLength(20), Display(Name ="Category")]
        public string CategoryId { get; set; }

        [Display(Name="Sub Category")]
        public int SubCategoryId { get; set; }

        [DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

    }

    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SpicyRating
    {
        [Display(Name = "N/A")]
        NA,
        [Display(Name = "Not Spicy")]
        NotSpicy,
        [Display(Name = "Spicy")]
        Spicy,
        [Display(Name = "Very Spicy")]
        VerySpicy,
        [Display(Name = "Red Hot")]
        RedHot
    }

}
