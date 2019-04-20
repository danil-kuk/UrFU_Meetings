using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Неверный формат")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Remote("PasswordCheck", "Login", HttpMethod = "Post", ErrorMessage = "Проверьте правильность введеного пароля")]
        public string Password { get; set; }
    }
}
