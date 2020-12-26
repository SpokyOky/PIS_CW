﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Logic.Models
{
    [DataContract]
    public class Division
    {
        [DataMember]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название.")]
        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey("DivisionId")]
        public virtual List<User> Users { get; set; }

        [ForeignKey("DivisionId")]
        public virtual List<Consignment> Consignments { get; set; }
    }
}
