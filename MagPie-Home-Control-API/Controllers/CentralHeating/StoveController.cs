using MagPie_Home_Control_API.Enums;
using MagPie_Home_Control_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagPie_Home_Control_API.Controllers.CentralHeating
{
    [Route("centralheating/stove")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly string _ewelinkProxyUrl;
        private readonly string _ewelinkDeviceId;
        private readonly HttpClient _httpClient;

        public StateController(IConfiguration configuration, HttpClient httpClient)
        {
            _ewelinkProxyUrl = configuration["EWELINK_PROXY_URL"] ?? throw new EnvironmentVariableMissingException("EWELINK_PROXY_URL");
            _ewelinkDeviceId = configuration["EWELINK_DEVICE_ID"] ?? throw new EnvironmentVariableMissingException("EWELINK_DEVICE_ID");
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get stove power state
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(GroupName = "Central heating")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var ewelinkResponse = await _httpClient.GetStringAsync($"{_ewelinkProxyUrl}/state/{_ewelinkDeviceId}");

                if (ewelinkResponse == "on")
                {
                    return Ok(new StateResponse(PowerStates.ON));
                }
                else if (ewelinkResponse == "off")
                {
                    return Ok(new StateResponse(PowerStates.OFF));
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

        /// <summary>
        /// Toggle stove power state
        /// </summary>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Central heating")]
        public async Task<IActionResult> Post()
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
