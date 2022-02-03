using Microsoft.AspNetCore.Mvc;
using WebStore.Services;

namespace WebStore.WebApi.Controllers
{
    [Route(WebAddresses.Values)]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        private readonly Dictionary<int, string> _Values = Enumerable.Range(1,10)
            .Select(i => (Id: i, Value: $"Value-{i}"))
            .ToDictionary(v => v.Id, v => v.Value);

        public ValuesController(ILogger<ValuesController> logger) => _logger = logger;

        [HttpGet]
        public IActionResult Get() => Ok(_Values.Values);

        [HttpGet("{Id}")]
        public IActionResult GetByid(int id)
        {
            if (!_Values.ContainsKey(id))
                return NotFound();
            return Ok(_Values[id]);
        }

        [HttpGet("count")]
        public IActionResult Count() => Ok(_Values.Count);

        [HttpPost("add")]
        public IActionResult Add(string value)
        {
            var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;
            _Values[id] = value;
            return CreatedAtAction(nameof(GetByid), new {id});
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] string value)
        {
            if (!_Values.ContainsKey(id))
                return NotFound();
            _Values[id] = value;
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_Values.ContainsKey(id))
                return NotFound();
            _Values.Remove(id);
            return Ok();
        }
    }
}