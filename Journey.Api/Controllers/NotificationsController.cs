using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Journey.Api.Controllers
{
    public class NotificationsController : BaseJourneyController
    {
        public NotificationsController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {
        }

        
    }
}
