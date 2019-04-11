using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApp.Models.DataModels;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Helpers
{
    public class EmailSender
    {        
        public void SendEmail(User user)
        {
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.mail.ru",
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("urfu-meetings@mail.ru", "EHaEdcnhtxb")
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("urfu-meetings@mail.ru"),
                Subject = "Регистрация в сервисе УрФУ Встречи",
                Body = "Спасибо, что зарегистрировались в нашем сервисе!",
                To = { user.Email },
                IsBodyHtml = true
            };

            client.Send(mailMessage);
            client.Dispose();
        }
    }
}
