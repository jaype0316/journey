using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Notifications
{
    internal class NotificationSubject
    {
        public string UserId { get; set; }
        //Push-Token
        public string RegistrationId { get; set; }
        public string Source { get; set; }
    }
}
