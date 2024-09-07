using MagPie_Home_Control_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagPie_Home_Control_API.Controllers.CentralHeating.Stove
{
    [Route("centralheating/stove/toggle")]
    [ApiController]
    public class ToggleController : ControllerBase
    {
        private readonly string _ewelinkProxyUrl;
        private readonly string _ewelinkDeviceId;
        private readonly HttpClient _httpClient;

        public ToggleController(IConfiguration configuration, HttpClient httpClient)
        {
            _ewelinkProxyUrl = configuration["EWELINK_PROXY_URL"] ?? throw new EnvironmentVariableMissingException("EWELINK_PROXY_URL");
            _ewelinkDeviceId = configuration["EWELINK_DEVICE_ID"] ?? throw new EnvironmentVariableMissingException("EWELINK_DEVICE_ID");
            _httpClient = httpClient;
        }

        /// <summary>
        /// Toggle stove power state
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(GroupName = "Central heating")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var ewelinkResponse = await _httpClient.GetStringAsync($"{_ewelinkProxyUrl}/toggle/{_ewelinkDeviceId}");

                if (ewelinkResponse == "OK")
                {
                    return Ok(new ApiResponseBody(true, "Stove power state has been toggled."));
                }
                else
                {
                    throw new ServerErrorException($"Invalid response from eWeLink: {ewelinkResponse}");
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseBody(false, e.Message));
            }
        }
    }
}
