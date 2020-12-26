using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Logic.Models
{
    public class Client
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести название.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести тип клиента.")]
        [StringLength(100)]
        public string Type { get; set; }

        [ForeignKey("ClientId")]
        public virtual List<Sale> Sales { get; set; }
    }
}
