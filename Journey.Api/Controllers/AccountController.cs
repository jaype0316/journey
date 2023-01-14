using AutoMapper;
using Journey.Api.Auth;
using Journey.Api.Models;
using Journey.Api.ViewModels;
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
        readonly SignInManager<AppUser> _signInManager;
        private readonly IApiAuthenticationTokenProvider _tokenProvider;

        public AccountController(IMediator mediator, IBlobStorageService blobStorage, IBlobKeyProvider keyProvider, 
                                IMapper mapper, IConfiguration config, UserManager<AppUser> userManager, 
                                SignInManager<AppUser> signInManager, IApiAuthenticationTokenProvider tokenProvider) :base(mediator, mapper)
        {
            this._blobStorage = blobStorage;
            this._blobObjectKeyProvider = keyProvider;
            this._config = config;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenProvider = tokenProvider;
        }


        [HttpGet, Route("AssertUser")]
        public async Task<Journey.Models.User> AssertUserExists()
        {
            var user = GetLoggedInUser();
            var assertedUser = await _mediator.Send(new GetOrAddRequest(user.UserId, user.Email));
            return assertedUser;
            //return await _mediator.Send(new GetBookRequest() { UserId = _userId});
        }

        [HttpPost, Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistration user)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if(existingUser != null)
            {
                ModelState.AddModelError("errors", $"{user.Email} already exists. Please login instead");
                return BadRequest(ModelState);
            }

            var appUser = new AppUser()
            {
                UserName = user.Email,
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

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLogin login)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                ModelState.AddModelError("authFailures", "Login Failed: Invalid Email or password");
                return BadRequest(ModelState);
            }
            
            await _signInManager.SignOutAsync();
            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("authFailures", "Login Failed: Invalid Email or password");
                return BadRequest(ModelState);
            }

            
            var token = _tokenProvider.Provide(user);

            //login successful
            //create jwt and return it
            return Ok(new { token = token });

        }

        [HttpGet, Route("UserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var user = GetLoggedInUser();
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            var userProfile = _mapper.Map<UserProfile>(appUser);

            return new JsonResult(userProfile);
        }

        [HttpPost, Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Ok();
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
