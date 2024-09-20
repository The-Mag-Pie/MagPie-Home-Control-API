using MagPie_Home_Control_API.Enums;
using MagPie_Home_Control_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagPie_Home_Control_API.Controllers.Room
{
    [Route("room/ledlighting")]
    [ApiController]
    public class LedLightingController : ControllerBase
    {
        private readonly string _homeAutomationControllerUrl;
        private readonly HttpClient _httpClient;

        private string StateURL => $"{_homeAutomationControllerUrl}/lighting/state";
        private string ToggleURL => $"{_homeAutomationControllerUrl}/lighting/switch";

        public LedLightingController(IConfiguration configuration, HttpClient httpClient)
        {
            _homeAutomationControllerUrl = configuration["HOME_AUTOMATION_CONTROLLER_URL"] ?? throw new EnvironmentVariableMissingException("HOME_AUTOMATION_CONTROLLER_URL");
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get LED lighting state
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(GroupName = "Room")]
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
        /// Toggle LED lighting state
        /// </summary>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Room")]
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
            var stateResponse = await _httpClient.GetStringAsync(StateURL);

            if (stateResponse == "ON")
            {
                return PowerStates.ON;
            }
            else if (stateResponse == "OFF")
            {
                return PowerStates.OFF;
            }
            else
            {
                throw new ServerErrorException($"Invalid response from home automation controller: {stateResponse}");
            }
        }

        private async Task<ToggleResponse> ToggleState()
        {
            var oldState = await GetState();
            var toggleResponse = await _httpClient.GetStringAsync(ToggleURL);

            if (toggleResponse == "OK")
            {
                return new ToggleResponse(
                    oldState,
                    oldState == PowerStates.ON ? PowerStates.OFF : PowerStates.ON);
            }
            else
            {
                throw new ServerErrorException($"Invalid response from home automation controller: {toggleResponse}");
            }
        }
    }
}
