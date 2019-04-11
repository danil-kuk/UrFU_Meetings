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
        private readonly EmailSettings _emailConfig;

        public EmailSender(IOptions<EmailSettings> options)
        {
            _emailConfig = new EmailSettings
            {
                Host = options.Value.Host,
                Port = options.Value.Port,
                UserName = options.Value.UserName,
                EnableSSL = options.Value.EnableSSL,
                Password = options.Value.Password
            };
        }

        public void SendEmail(string userEmail, string subject, string text)
        {
            SmtpClient client = new SmtpClient
            {
                Host = _emailConfig.Host,
                Port = _emailConfig.Port,
                EnableSsl = _emailConfig.EnableSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password)
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_emailConfig.UserName),
                Subject = subject,
                Body = text,
                To = { userEmail },
                IsBodyHtml = true
            };

            client.Send(mailMessage);
            client.Dispose();
        }
    }
}
