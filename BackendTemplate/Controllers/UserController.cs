using Azure;
using ClinicSystem.Dtos;
using Domain.Dtos.User;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackendTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IjwtService _jwtService;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserController(IjwtService jwtService, IHttpContextAccessor contextAccessor)
        {
            _jwtService = jwtService;
            _contextAccessor = contextAccessor;
        }
        [HttpPost("LogIn")]
        public ActionResult LogIn(LoginViewModel input)
        {
            var response = _jwtService.GenerateToken(input);
            if(response==null)
            {
                return BadRequest(new ApiResponse<string>(errorMsg:"UserName or password invalid"));
            }
            return Ok(response);
        }

        [HttpPost("SignUp")]
        public ActionResult SignUp(CreateUser input)
        {
            var response = _jwtService.CreateUser(input);
            if (response == null)
            {
                return BadRequest(new ApiResponse<string>("Can not SignUp, Contact with Administrator"));
            }
            return Ok(new ApiResponse<CreateUser>(response));
        }

        [HttpGet("GetProfile")]
        [Authorize(Roles ="Normal")]
        public ActionResult GetProfile()
        {
            CurrentUserViewModel user = new CurrentUserViewModel();

            user.UserId = new Guid(User.FindFirst(ClaimTypes.Sid)?.Value);
            user.UserName = User.FindFirst(ClaimTypes.Name)?.Value;
            user.RoleName = User.FindFirst(ClaimTypes.Role)?.Value;

            if (user == null)
            {
                return Unauthorized();
            }
           
            return Ok(new ApiResponse<CurrentUserViewModel>(user));
        }

        [HttpPost("GrantUserPermission")]
        [Authorize(Roles ="Admin")]
        public ActionResult GrantUserPermission(Guid userid)
        {
            var response = _jwtService.GrantUserPermission(userid);
            if (response == null)
            {
                return BadRequest(new ApiResponse<CurrentUserViewModel>("User Invalid"));
            }
            return Ok(new ApiResponse<CurrentUserViewModel>(response));
        }

        [HttpGet("refresh-token")]
        [AllowAnonymous]
        public ActionResult RefreshToken()
        {
            string Header = _contextAccessor.HttpContext.Request.Headers["Authorization"];
            if (Header == null)
            {
                return Unauthorized();
            }
            var token = Header.Split(' ').Last();
            var principal = _jwtService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity?.Name;
            if(!_jwtService.CheckExpiredCookiesRefreshToken())
            {
                return Unauthorized("Invalid attempt!");
            }
            LoginViewModel input = new LoginViewModel();
            input.UserName = username;
            input.Password=_jwtService.GetClairPassword(username);
            var response = _jwtService.GenerateToken(input);
            if (response == null)
            {
                return BadRequest(new ApiResponse<string>(errorMsg: "UserName or password invalid"));
            }
            return Ok(response);

        }


    }
}
