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

        private string StateURL => $"{_ewelinkProxyUrl}/state/{_ewelinkDeviceId}";
        private string ToggleURL => $"{_ewelinkProxyUrl}/toggle/{_ewelinkDeviceId}";

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
                return Ok(new StateResponse(await GetState()));
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
                return Ok(await ToggleState());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseBody(false, e.Message));
            }
        }

        private async Task<PowerStates> GetState()
        {
            var ewelinkResponse = await _httpClient.GetStringAsync(StateURL);

            if (ewelinkResponse == "on")
            {
                return PowerStates.ON;
            }
            else if (ewelinkResponse == "off")
            {
                return PowerStates.OFF;
            }
            else
            {
                throw new ServerErrorException($"Invalid response from eWeLink: {ewelinkResponse}");
            }
        }

        private async Task<ToggleResponse> ToggleState()
        {
            var oldState = await GetState();
            var ewelinkResponse = await _httpClient.GetStringAsync(ToggleURL);

            if (ewelinkResponse == "OK")
            {
                return new (
                    oldState,
                    oldState == PowerStates.ON ? PowerStates.OFF : PowerStates.ON);
            }
            else
            {
                throw new ServerErrorException($"Invalid response from eWeLink: {ewelinkResponse}");
            }
        }
    }
}
