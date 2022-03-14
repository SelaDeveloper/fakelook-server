using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var dbUser = _userRepository.GetById(user.Id); //need to check username and password
            if (dbUser == null) return Problem("user not in system");
            var token = _tokenService.CreateToken(dbUser);
            return Ok(new { token });
        }
        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp([FromBody] User user)
        {
            var dbUser =  _userRepository.Add(user).Result;
            var token = _tokenService.CreateToken(dbUser);
            return Ok(new { token });
        }

    }
}
