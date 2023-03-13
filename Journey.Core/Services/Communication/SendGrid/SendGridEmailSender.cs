using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Communication.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridSettings _settings;
        public SendGridEmailSender(IOptions<SendGridSettings> sendGridSettings)
        {
            this._settings = sendGridSettings.Value;
        }
        public async Task SendEmailAsync(string target, string subject, string body, string bodyHtml)
        {
            var client = new SendGridClient(_settings.ApiKey);
            var from = new EmailAddress(_settings.SenderEmail, _settings.SenderName);
            var to = new EmailAddress(target);
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = new SendGridMessage()
            {
                From = from,
                Subject = subject,
                HtmlContent = bodyHtml
            };
            msg.AddTo(target);
            msg.SetClickTracking(false, false);
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, null, bodyHtml);
            
            var response = await client.SendEmailAsync(msg);
        }
    }
}
