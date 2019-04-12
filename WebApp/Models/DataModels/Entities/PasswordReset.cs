using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.DataModels.Entities
{
    public class PasswordReset
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

        [Required]
        [MaxLength(200)]
        public string ActivationKey { get; set; }
    }
}
