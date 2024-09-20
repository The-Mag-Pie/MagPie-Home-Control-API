using MagPie_Home_Control_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MagPie_Home_Control_API.Controllers.Room
{
    [Route("room/climate")]
    [ApiController]
    public class ClimateController : ControllerBase
    {
        private readonly string _homeAutomationControllerUrl;
        private readonly HttpClient _httpClient;

        private string TemperatureURL => $"{_homeAutomationControllerUrl}/temperature";
        private string HumidityURL => $"{_homeAutomationControllerUrl}/humidity";

        public ClimateController(IConfiguration configuration, HttpClient httpClient)
        {
            _homeAutomationControllerUrl = configuration["HOME_AUTOMATION_CONTROLLER_URL"] ?? throw new EnvironmentVariableMissingException("HOME_AUTOMATION_CONTROLLER_URL");
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get room climate
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(GroupName = "Room")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var temperatureResponse = await _httpClient.GetStringAsync(TemperatureURL);
                var humidityResponse = await _httpClient.GetStringAsync(HumidityURL);

                return Ok(new ApiResponseBody(true, null, new
                {
                    Temperature = Convert.ToDouble(temperatureResponse, CultureInfo.InvariantCulture),
                    Humidity = Convert.ToDouble(humidityResponse, CultureInfo.InvariantCulture)
                }));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseBody(false, e.Message));
            }
        }
    }
}
