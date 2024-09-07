using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagPie_Home_Control_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Test endpoint
        /// </summary>
        /// <returns>Test response</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_configuration["TEST_ENV"]);
        }
    }
}
