using AutoMapper;
using Journey.Api.Models;
using Journey.Core.Providers;
using Journey.Core.Repository;
using Journey.Core.Services.Blobs;
using Journey.Core.Services.Books;
using Journey.Core.Services.Chapters;
using Journey.Core.Services.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Journey.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseJourneyController
    {
        private readonly IBlobStorageService _blobStorage;
        private readonly IBlobKeyProvider _blobObjectKeyProvider;
        readonly IConfiguration _config;
        readonly UserManager<AppUser> _userManager;

        public AccountController(IMediator mediator, IBlobStorageService blobStorage, IBlobKeyProvider keyProvider, 
                                IMapper mapper, IConfiguration config, UserManager<AppUser> userManager) :base(mediator, mapper)
        {
            this._blobStorage = blobStorage;
            this._blobObjectKeyProvider = keyProvider;
            this._config = config;
            this._userManager = userManager;
        }


        [HttpGet, Route("AssertUser")]
        public async Task<Journey.Models.User> AssertUserExists()
        {
            var user = GetLoggedInUser();
            var assertedUser = await _mediator.Send(new GetOrAddRequest(user.UserId, user.Email));
            return assertedUser;
            //return await _mediator.Send(new GetBookRequest() { UserId = _userId});
        }

        public async Task<IActionResult> Register(User user)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = new AppUser()
            {
                UserName = user.Name,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return Ok(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] UserRegistration user)
        {
            if (user == null)
                return BadRequest();

            //temp
            if(user.Email == "jaype0316@gmail.com" && user.Password == "colombia2")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._config.GetSection("SigningKey").Value));
                var creds  = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "", 
                    audience: "", 
                    claims: new List<Claim>(), 
                    expires: DateTime.UtcNow.AddMinutes(60), 
                    signingCredentials: creds
                );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { Token = jwt });
            }

            return Unauthorized();
        }

        [HttpGet, Route("Bootstrap")]
        public async Task<IActionResult> Bootstrap()
        {
            var user = GetLoggedInUser();
            await _mediator.Send(new Core.Services.User.GetOrAddRequest(user.UserId, user.Email));

            var book = await _mediator.Send(new Core.Services.Books.GetBookRequest(user.UserId));
            if(book == null)
                book = await _mediator.Send(new SaveBookCommand(user.UserId) { Title = "My Journey", About = "What's on your mind...?", UserId = user.UserId });
            
            return new JsonResult(book);

        }

    }
}
