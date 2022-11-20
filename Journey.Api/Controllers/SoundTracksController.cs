using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Journey.Core.Services.Blobs;

namespace Journey.Api.Controllers
{
    [Route("api/soundtrack")]
    [ApiController]
    [Authorize]
    public class SoundTracksController : BaseJourneyController
    {
        readonly IBlobStorageService _blobs;
        public SoundTracksController(IMediator mediator, IMapper mapper, IBlobStorageService blobStorageService) : base(mediator, mapper)
        {
            _blobs = blobStorageService;
        }

        [HttpGet, Route("download")]
        public async Task<IActionResult> Download(int? skip = 0, int take = 50)
        {
            return new JsonResult(await _blobs.GetList("soundtracks", null, take));
        }
    }
}
