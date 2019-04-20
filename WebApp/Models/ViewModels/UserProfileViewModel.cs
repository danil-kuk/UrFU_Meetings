using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.ViewModels
{
    public class UserProfileViewModel : RegistrationViewModel
    {
        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Remote("PasswordCheckBeforeDelete", "UserProfile", HttpMethod = "Post", ErrorMessage = "Неверный пароль")]
        public string DeleteConfirm { get; set; }
    }
}
