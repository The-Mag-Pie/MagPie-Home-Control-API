using MagPie_Home_Control_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagPie_Home_Control_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private class ResponseModel
        {
            public int RandomTestInt { get; set; }
        }

        /// <summary>
        /// Check if the server is working
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(GroupName = "Health check")]
        public IActionResult Get()
        {
            return Ok(new ApiResponseBody(true, "SERVER IS WORKING", new
            {
                RandomInteger = new Random().Next(-100, 100)
            }));
        }
    }
}
