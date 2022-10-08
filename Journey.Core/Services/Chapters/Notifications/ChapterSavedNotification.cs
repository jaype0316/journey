using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Chapters.Notifications
{
    public class ChapterSavedNotification : INotification
    {
        public string UserId { get; set; }

        public ChapterSavedNotification()
        {

        }
        public ChapterSavedNotification(string userId)
        {
            UserId = UserId;
        }
    }
}
