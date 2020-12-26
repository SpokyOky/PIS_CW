using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Logic.Models
{
    [DataContract]
    public class TableProduct
    {
        public int? Id { get; set; }

        [DataMember]
        public int Amount { get; set; }

        [DataMember]
        [Required]
        public virtual Product Product { get; set; }

        public virtual Consignment Consignment { get; set; }

        public virtual Sale Sale { get; set; }
    }
}
