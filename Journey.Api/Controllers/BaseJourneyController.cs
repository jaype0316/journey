using AutoMapper;
using Journey.Api.Models;
using Journey.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

            var bearerToken = HttpContext.Request.Headers["Authorization"].ToString();
            var jwt = bearerToken.Replace("Bearer", "").Trim();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(jwt);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var context = UserContextCache.Get(userId);
            return (context.UserId, context.Email);
        }
    }
}
