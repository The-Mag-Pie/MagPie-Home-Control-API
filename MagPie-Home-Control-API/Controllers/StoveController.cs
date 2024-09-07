using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagPie_Home_Control_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StoveController : ControllerBase
    {
        private readonly string _ewelinkProxyUrl;
        private readonly string _ewelinkDeviceId;
        private readonly HttpClient _httpClient;

        public StoveController(IConfiguration configuration, HttpClient httpClient)
        {
            _ewelinkProxyUrl = configuration["EWELINK_PROXY_URL"] ?? throw new EnvironmentVariableMissingException("EWELINK_PROXY_URL");
            _ewelinkDeviceId = configuration["EWELINK_DEVICE_ID"] ?? throw new EnvironmentVariableMissingException("EWELINK_DEVICE_ID");
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://onet.pl");
            var response = await _httpClient.SendAsync(request);
            return Ok(await response.Content.ReadAsStringAsync());
        }
    }
}
