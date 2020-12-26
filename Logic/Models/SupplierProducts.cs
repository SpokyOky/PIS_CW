using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Logic.Models
{
    public class SupplierProducts
    {
        [Required]
        public virtual Supplier Supplier { get; set; }
        [Required]
        public virtual ProductGroup ProductGroup { get; set; }
    }
}
