   
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
 
    {
        public IConfiguration _configuration { get; }
        public ConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet(Name ="getConfiguration")]
        public IActionResult getConfiguration()
        {
            
            return Ok(_configuration["name"]);
        }
    }
}
