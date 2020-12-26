using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Logic.Models
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public int? Id { get; set; }

        [Required(ErrorMessage ="Необходимо ввести название.")]
        [StringLength(100)]
        [DisplayName("Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести марку.")]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Необходимо ввести производителя.")]
        [StringLength(100)]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Необходимо ввести рекламную информацию.")]
        [StringLength(255)]
        public string AdvertData { get; set; }

        [Required(ErrorMessage = "Необходимо ввести город.")]
        [StringLength(100)]
        public string City { get; set; }

        [DataMember]
        public double Price { get; set; }

        [Required]
        public virtual ProductGroup ProductGroup { get; set; }

        [ForeignKey("ProductId")]
        public virtual List<TableProduct> TableProducts { get; set; }
    }
}
