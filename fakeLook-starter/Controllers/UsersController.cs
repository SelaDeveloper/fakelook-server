using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fakeLook_starter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userRepository.GetAll();
        }

        // GET api/<UsersController>/5
        [HttpGet("{UserName}")]
        public User Get(string userName)
        {
            return _userRepository.GetByUserName(userName);
        }

        // POST api/<UsersController>
        //[HttpPost]
        //public async Task<ActionResult<User>> Post([FromBody] User user)
        //{
        //    return await _userRepository.Add(user);
        //}

        //put api/<userscontroller>/5
        [HttpPut("userName")]
        public User ChangePassword(string userName, string newPassword)
        {
            return _userRepository.ChangePassword(userName, newPassword).Result;
        }

        // DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
