using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int MenuItemId { get; set; }

        [Required, Range(1,12)]
        public int Quantity { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }
        
        // Navigation Properties

        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }

        [ForeignKey("OrderId")]
        public virtual OrderHeader Order { get; set; }
    }
}
