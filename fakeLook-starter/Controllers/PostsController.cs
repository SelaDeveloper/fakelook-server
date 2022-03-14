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
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        // GET: api/<PostsController>
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _postRepository.GetAll();
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public Post Get(int id)
        {
            return _postRepository.GetById(id);
        }

        // POST api/<PostsController>
        [HttpPost]
        public async Task<ActionResult <Post>> Post ([FromBody] Post post)
        {
            return await _postRepository.Add(post);
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Post>> Put(int id, [FromBody] Post post)
        {
            return await _postRepository.Edit(post);
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> Delete(int id)
        {
            return await _postRepository.Delete(id);
        }

        
    }
}
