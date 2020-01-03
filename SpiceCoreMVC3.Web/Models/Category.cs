using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceCoreMVC3.Web.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(20), DisplayName("Name")]
        public string Id { get; set; }

        [Range(1,20), DisplayName("Menu Order")]
        public Nullable<int> MenuOrder { get; set; }
    }
}
