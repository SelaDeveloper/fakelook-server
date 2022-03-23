using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using fakeLook_starter.Utilities;
using fakeLook_starter.Filter;

namespace fakeLook_starter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserRepository _userRepository;
        private ITokenService _tokenService { get; }

        public AuthController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }


        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] User user)
        {
            var dbUser = _userRepository.GetByUserName(user.UserName); //need to check username and password
            if (dbUser == null) return Problem("user not in system");
            if (dbUser.Password == Encryptions.MyEncryption(user.Password))
            {
                var token = _tokenService.CreateToken(dbUser);
                return Ok(new { token, dbUser.Id });
            }
            return Problem("Invalid Password");
        }
        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp([FromBody] User user)
        {
            var isUserExist = _userRepository.GetByUserName(user.UserName);
            if (isUserExist == null)
            {
                var dbUser = _userRepository.Add(user).Result;
                var token = _tokenService.CreateToken(dbUser);
                return Ok(new { token, dbUser.Id });
            }
            else
            {
                return Problem("user already exist");
            }
        }

        [HttpGet]
        [Route("TestAll")]
        [TypeFilter(typeof(GetUserActionFilter))]

        public IActionResult TestAll()
        {
            return Ok();
        }

        [HttpPut("userName")]
        public User ChangePassword(string userName, string newPassword)
        {
            return _userRepository.ChangePassword(userName, newPassword).Result;
        }

    }
}
