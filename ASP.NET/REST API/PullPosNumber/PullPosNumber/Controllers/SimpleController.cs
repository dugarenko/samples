using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PullPosNumber.Filters;
using PullPosNumber.Properties;
using System;
using System.Linq;

namespace PullPosNumber.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    //[TokenAuthorizationFilter]
    public class SimpleController : ControllerBase
    {
        private readonly ILogger<SimpleController> _logger;
        private readonly string[] _computerNames = new string[] { "KOMPUTER_1", "KOMPUTER_2", "KOMPUTER_3" };

        public SimpleController(ILogger<SimpleController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/rest/dkz029/channel/posNumber/{ComputerName}")]
        public IActionResult Get(string computerName)
        {
            if (string.IsNullOrEmpty(computerName) ||
                !_computerNames.Contains(computerName.ToUpper()))
            {
                return BadRequest(Resources.Error_400_Html);
            }

            Response.Cookies.Append("Set-Cookie", computerName);
            return new RedirectResult("/Redirect.html", false);
        }
    }
}
