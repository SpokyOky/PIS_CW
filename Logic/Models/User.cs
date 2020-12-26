using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Logic.Models
{
    public class User
    {
        public int? Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }

        public string Name { get; set; }

        public virtual Division Division { get; set; }
    }
}
