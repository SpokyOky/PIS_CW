using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Logic.Models
{
    [DataContract]
    public class Supplier
    {
        [DataMember]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести ИНН.")]
        [StringLength(100)]
        public string INN { get; set; }

        public virtual List<ProductGroup> ProductGroups { get; set; }

        [ForeignKey("SupplierId")]
        public virtual List<Consignment> Consignments { get; set; }
    }
}
