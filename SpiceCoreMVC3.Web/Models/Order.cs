using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpiceCoreMVC3.Web.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy.mm.dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOrdered { get; set; }

        public string CustomerID { get; set; }

        [Required]
        public int MenuItemID { get; set; }

        [Required, Range(1,12, ErrorMessage ="Please enter how many servings between 1 and 12")]
        public int Quantity { get; set; }

        // Navigation Properties

        [ForeignKey("CustomerID")]
        public virtual ApplicationUser Customer { get; set; }

        [ForeignKey("MenuItemID")]
        public virtual MenuItem MenuItem { get; set; }

        public Order()
        {
            Quantity = 1;
            DateOrdered = DateTime.Now;
        }
    }
}
