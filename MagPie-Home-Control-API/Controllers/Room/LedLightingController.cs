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
                var response = await _httpClient.GetStringAsync($"{_homeAutomationControllerUrl}/lighting/state");

                if (response == "ON")
                {
                    return Ok(new StateResponse(PowerStates.ON));
                }
                else if (response == "OFF")
                {
                    return Ok(new StateResponse(PowerStates.OFF));
                }
                else
                {
                    throw new ServerErrorException($"Invalid response from home automation controller: {response}");
                }
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
                var stateResponse = await _httpClient.GetStringAsync($"{_homeAutomationControllerUrl}/lighting/state");

                if (stateResponse != "ON" && stateResponse != "OFF")
                {
                    throw new ServerErrorException($"Invalid response from home automation controller: {stateResponse}");
                }

                var oldState = stateResponse == "ON" ? PowerStates.ON : PowerStates.OFF;
                var newState = oldState == PowerStates.ON ? PowerStates.OFF : PowerStates.ON;

                var toggleResponse = await _httpClient.GetStringAsync($"{_homeAutomationControllerUrl}/lighting/switch");

                if (toggleResponse != "OK")
                {
                    throw new ServerErrorException($"Invalid response from home automation controller: {toggleResponse}");
                }

                return Ok(new ApiResponseBody(true, toggleResponse, new
                {
                    OldState = oldState,
                    NewState = newState
                }));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseBody(false, e.Message));
            }
        }
    }
}
