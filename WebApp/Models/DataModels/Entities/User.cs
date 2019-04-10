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
        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Имя должно состоять только из русских букв")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [RegularExpression(@"^[а-яА-Я]+$", ErrorMessage = "Фамилия должна состоять только из русских букв")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Неверный формат")]
        [Remote("EmailCheck", "Registration", HttpMethod = "Post", ErrorMessage = "Такая почта уже зарегистрирована, используйте другую")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, ErrorMessage = "Пароль должен быть не короче {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
