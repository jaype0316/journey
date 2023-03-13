using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Communication
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string target, string subject, string body, string bodyHtml);
    }
}
