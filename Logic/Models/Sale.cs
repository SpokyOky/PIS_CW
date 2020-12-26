using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Logic.Models
{
    public class Sale
    {
        public int? Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        public double Sum { get; set; }

        [ForeignKey("SaleId")]
        public virtual List<TableProduct> TableProducts { get; set; }

        public virtual Client Client { get; set; }
    }
}
