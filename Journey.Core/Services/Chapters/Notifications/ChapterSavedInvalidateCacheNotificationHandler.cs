using Journey.Core.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Chapters.Notifications
{
    public class ChapterSavedInvalidateCacheNotificationHandler : INotificationHandler<ChapterSavedNotification>
    {
        readonly ICacheProvider _cache;
        public ChapterSavedInvalidateCacheNotificationHandler(ICacheProvider cache)
        {
            _cache = cache;
        }

        public Task Handle(ChapterSavedNotification notification, CancellationToken cancellationToken)
        {
            //todo: decouple the cache key knowledge from here
            return _cache.Invalidate($"Book_{notification.UserId}");
        }
    }
}
