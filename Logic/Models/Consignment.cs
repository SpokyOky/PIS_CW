using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Logic.Models
{
    [DataContract]
    public class Consignment
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        [Required]
        public string Type { get; set; }

        [DataMember]
        [Required]
        public DateTime Date { get; set; }

        [DataMember]
        public double Sum { get; set; }

        [DataMember]
        public virtual Stock Stock { get; set; }

        [DataMember]
        [ForeignKey("ConsignmentId")]
        public virtual List<TableProduct> TableProducts { get; set; }

        [DataMember]
        public virtual Division Division { get; set; }

        [DataMember]
        public virtual Supplier Supplier { get; set; }
    }
}
