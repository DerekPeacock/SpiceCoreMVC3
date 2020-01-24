using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string UserId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime PickupTime { get; set; }

        [Required, NotMapped, DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency)]
        public decimal Discount { get; set; }

        [StringLength(30), Display(Name ="Pickup Name")]
        public string PickupName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(250), DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [Display(Name ="Order Status")]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }

        [NotMapped]
        public decimal TotalCost { get; set; }

        [StringLength(60)]
        public string TransactionId { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public OrderHeader()
        {
            OrderDate = DateTime.Now;
            PaymentStatus = PaymentStatus.NOT_PAID;
            OrderStatus = OrderStatus.STARTED;
            Discount = 0;
            TotalCost = 0;
        }
    }

    public enum OrderStatus
    {
        STARTED,
        PLACED,
        IN_PROGRESS,
        READY,
        DELEVERED
    }

    public enum PaymentStatus 
    { 
        NOT_PAID,
        PAID,
        REJECTED
    }
}
