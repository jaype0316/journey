using AutoMapper;
using Journey.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagsController : BaseJourneyController
    {
        public TagsController(IMediator mediator, IMapper mapper) : base(mediator, mapper)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = GetLoggedInUser();
            var tags = await _mediator.Send(new Core.Services.Tags.GetRequest(user.UserId));
            return new JsonResult(tags);
        }

        [HttpPost]
        public async Task<IActionResult> Save(IEnumerable<UserTag.Tag> tags)
        {
            var user = GetLoggedInUser();
            await _mediator.Send(new Core.Services.Tags.SaveCommand(user.UserId, tags));
            return Ok();
        }

    }
}
