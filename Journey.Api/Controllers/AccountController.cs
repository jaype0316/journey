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
using Journey.Core.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
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
        readonly ICacheProvider _cacheProvider;
        private readonly IApiAuthenticationTokenProvider _tokenProvider;

        public AccountController(IMediator mediator, IBlobStorageService blobStorage, IBlobKeyProvider keyProvider, 
                                IMapper mapper, IConfiguration config, UserManager<AppUser> userManager, 
                                SignInManager<AppUser> signInManager, IApiAuthenticationTokenProvider tokenProvider, 
                                ICacheProvider cacheProvider) :base(mediator, mapper)
        {
            this._blobStorage = blobStorage;
            this._blobObjectKeyProvider = keyProvider;
            this._config = config;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenProvider = tokenProvider;
            this._cacheProvider = cacheProvider;
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
            var t = User.Identity.IsAuthenticated;

            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                ModelState.AddModelError("authFailures", "Login Failed: Invalid Email or password");
                return BadRequest(ModelState);
            }

            var t2 = User.Identity.IsAuthenticated;

            if(!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                ModelState.AddModelError("authFailures", "Login Failed: Invalid Email or password");
                return BadRequest(ModelState);
            }

            await _signInManager.SignOutAsync();
            //var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim("Id", user.Id));

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,new ClaimsPrincipal(identity));
            
            var token = _tokenProvider.Provide(user);

            //login successful
            //create jwt and return it
            return Ok(new { token = token });

        }

        [HttpPost, Route("Update")]
        public async Task<IActionResult> Update(AppUser user)
        {
            var identity = GetLoggedInUser();
            var appUser = await _userManager.FindByEmailAsync(identity.Email);
            appUser.FirstName = user.FirstName;
            appUser.LastName = user.LastName;
            appUser.AvatarUrl = user.AvatarUrl;

            var result =  await this._userManager.UpdateAsync(appUser);

            return result.Succeeded ? Ok(result) : BadRequest(result);
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
            var currentUser = GetLoggedInUser();
            await _cacheProvider.Invalidate($"Book_{currentUser.UserId}");
            await _signInManager.SignOutAsync();
            
            return Ok();
        }

        [HttpGet, Route("Bootstrap")]
        public async Task<IActionResult> Bootstrap()
        {
            var user = GetLoggedInUser();
            await _mediator.Send(new Core.Services.User.GetOrAddRequest(user.UserId, user.Email));

            //Seed an initial jounral
            var book = await _mediator.Send(new Core.Services.Books.GetBookRequest(user.UserId));
            if(book == null)
                book = await _mediator.Send(new SaveBookCommand(user.UserId) { Title = "My Journey", About = "What's on your mind...?", UserId = user.UserId });

            //Seed initial tags
            var userTags = await _mediator.Send(new Core.Services.Tags.GetRequest(user.UserId));
            if (userTags == null)
                await _mediator.Send(new Core.Services.Tags.SaveCommand(user.UserId, Core.Services.Tags.Defaults.Tags));
            
            return new JsonResult(book);

        }

        [HttpPost, Route("ProfileImage")]
        public async Task<IActionResult> ProfileImage(IFormFile file)
        {
            if (file.Length == 0) return new NoContentResult();
            
            var identity = GetLoggedInUser();
            var t = User.Identity;
            var blobObjectKey = _blobObjectKeyProvider.Provide($"{identity.UserId}-{file.FileName}");
            var success = await _blobStorage.Add(blobObjectKey, file);

            if (!success) return new NoContentResult();

            var blobUri = $"{_blobStorage.BlobBaseUri}{blobObjectKey}";
            await _cacheProvider.GetOrAdd<string>($"{User.Identity.Name}-avatar", async () => { return blobUri; });

            return new JsonResult(blobUri);

        }

    }
}
