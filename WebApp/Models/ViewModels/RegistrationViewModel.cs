using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Models.ViewModels
{
    public class RegistrationViewModel : User
    {
        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Compare("Password", ErrorMessage = "Пароли отличаются")]
        public string ConfirmPassword { get; set; }
    }
}
