using Journey.Core.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Books.Notifications
{
    public class BookSavedNotification : INotification
    {
        public string UserId { get; private set; }
        public BookSavedNotification(string userId)
        {
            this.UserId = userId;
        }

    }

    public class BookSavedNotificationHandler : INotificationHandler<BookSavedNotification>
    {
        readonly ICacheProvider _cacheProvider;
        public BookSavedNotificationHandler(ICacheProvider cacheProvider)
        {
            this._cacheProvider = cacheProvider;
        }
        public Task Handle(BookSavedNotification notification, CancellationToken cancellationToken)
        {
            return _cacheProvider.Invalidate($"Book_{notification.UserId}");
        }
    }
}
