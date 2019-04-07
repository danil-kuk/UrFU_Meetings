using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.DataModels.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Введите Email")]
        [Remote("EmailCheck", "SignIn", HttpMethod = "Post", ErrorMessage = "Такая почта уже зарегистрирована, используйте другую")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, ErrorMessage = "Пароль должен быть не короче {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
