using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20), Display(Name ="Sub Category Name")]
        public string Name { get; set; }

        [Required, StringLength(20), Display(Name ="Category")]
        public string CategoryId { get; set; }

        //

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
