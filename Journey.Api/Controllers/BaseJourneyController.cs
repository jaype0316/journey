using AutoMapper;
using Journey.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Api.Controllers
{
    public class BaseJourneyController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;

        public BaseJourneyController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            this._mapper = mapper;
        }
    
        protected (string? UserId, string? Email) GetLoggedInUser()
        {
            //todo:
            var id = User?.Claims?.FirstOrDefault(c => c.Type == "Id")?.Value;
            var email = User?.Claims?.FirstOrDefault(c => c.Type == "email")?.Value;
            return (id, email);
        }
    }
}
