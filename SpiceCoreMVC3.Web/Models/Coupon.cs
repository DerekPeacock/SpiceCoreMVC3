using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }

        public CouponTypes CouponType { get; set; }

        [DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        public decimal Discount { get; set; }

        [DataType(DataType.Currency), DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = false)]
        public decimal MinimumAmount { get; set; }

        public byte[] Picture { get; set; }

        public bool IsActive { get; set; }

        public Coupon()
        {
            IsActive = true;
            MinimumAmount = 0;
            Discount = 0;
            CouponType = CouponTypes.Percent;
        }
    }

    public enum CouponTypes 
    {
        Percent,
        Dollar
    }
}
