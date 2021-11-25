using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            this._emailConfig = emailConfig;
        }
        public async Task SendEmailAsync(Message message) => await SendAsync(CreateEmailMessage(message));

        private MailMessage CreateEmailMessage(Message message)
        {
            var toEmail = new MailAddress(message.To);
            var fromEmail = new MailAddress(_emailConfig.From, "Bump Application");
            var emailMessage = new MailMessage(fromEmail, toEmail)
            {
                Subject = message.Subject,
                IsBodyHtml = true,
                Body = string.Format("<h2 style='color: red;'>to reset your password click on this button</h2> <button><a href='{0}' target='_blank'>Reset Password</a></button>", message.Content)
            };
            return emailMessage;
        }

        private async Task SendAsync(MailMessage mailMessage)
        {
            
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_emailConfig.UserName, _emailConfig.Password);
                    client.Host = _emailConfig.SmtpServer;
                    client.Port = _emailConfig.Port;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
