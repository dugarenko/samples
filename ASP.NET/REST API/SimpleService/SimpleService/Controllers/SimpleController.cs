using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleService.Filters;
using System;

namespace SimpleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TokenAuthorizationFilter]
    public class SimpleController : ControllerBase
    {
        private readonly ILogger<SimpleController> _logger;

        public SimpleController(ILogger<SimpleController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }
            return new JsonResult(new { ComputerName = Environment.MachineName }, new JsonSerializerSettings() {Formatting = Formatting.Indented });
        }
    }
}
