using Microsoft.AspNetCore.Mvc;
using phrase_api.Contracts.Workers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace phrase_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IWordsWoker _words;

        public WordsController(IWordsWoker words)
        {
            _words = words;
        }

        // GET: api/<WordsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _words.GetParts();
        }

        // GET api/<WordsController>/5
        [HttpGet("{partOfSpeach}/{search}")]
        public IEnumerable<string> Get(string partOfSpeach,string search)
        {
            return _words.Search(partOfSpeach, search).Result;
        }
    }
}
