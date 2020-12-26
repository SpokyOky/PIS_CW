using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Logic.Models
{
    public class ProductGroup
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести норматив.")]
        [Range(0, int.MaxValue)]
        public int Norm { get; set; }

        [ForeignKey("ProductGroupId")]
        public virtual List<Product> Products { get; set; }

        public virtual List<Supplier> Suppliers { get; set; }
    }
}
