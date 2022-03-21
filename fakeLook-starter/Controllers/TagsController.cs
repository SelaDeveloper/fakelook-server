using fakeLook_models.Models;
using fakeLook_starter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fakeLook_starter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        readonly private ITagRepository _tagRepository;
        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        // GET: api/<TagsController>
        [HttpGet]
        public ICollection<Tag> Get()
        {
            return _tagRepository.GetAll();
        }

    }
}
