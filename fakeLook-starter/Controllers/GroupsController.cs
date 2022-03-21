//using fakeLook_starter.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace fakeLook_starter.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GroupsController : ControllerBase
//    {
//        private readonly IGroupRepository _groupRepository;
//        public GroupsController(IGroupRepository groupRepository)
//        {
//            _groupRepository = groupRepository;
//        }
//        // GET: api/<GroupsController>
//        [HttpGet]
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

//        // GET api/<GroupsController>/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/<GroupsController>
//        [HttpPost]
//        public void Post([FromBody] string value)
//        {
//        }

//        // PUT api/<GroupsController>/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE api/<GroupsController>/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}
