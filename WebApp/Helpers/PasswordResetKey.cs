using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;
using WebApp.Services.Interfaces;

namespace WebApp.Helpers
{
    public class PasswordResetKey
    {
        private readonly IPasswordResetService _passwordResetService;

        public PasswordResetKey(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        public string ActivationKey(string email)
        {
            string guid = Guid.NewGuid().ToString();
            while (_passwordResetService.GetByFilter(i => i.ActivationKey == guid) != null)
            {
                guid = Guid.NewGuid().ToString();
            }
            string key = email + ":OSK:" + DateTime.Now + ":OSK:" + guid;
            PasswordReset emailValid = new PasswordReset
            {
                Email = email,
                Time = DateTime.Now,
                ActivationKey = guid
            };
            _passwordResetService.Insert(emailValid);
            return new AESEncryption().EncryptText(key);
        }
    }
}
