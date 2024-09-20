using MagPie_Home_Control_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MagPie_Home_Control_API.Controllers.CentralHeating
{
    [Route("centralheating/watertemperature")]
    [ApiController]
    public class WaterTemperatureController : ControllerBase
    {
        private readonly string _waterTemperatureSensorUrl;
        private readonly HttpClient _httpClient;

        private string TemperatureURL => $"{_waterTemperatureSensorUrl}/temp";

        public WaterTemperatureController(IConfiguration configuration, HttpClient httpClient)
        {
            _waterTemperatureSensorUrl = configuration["WATER_TEMPERATURE_SENSOR_URL"] ?? throw new EnvironmentVariableMissingException("WATER_TEMPERATURE_SENSOR_URL");
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get water temperature
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(GroupName = "Central heating")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var temperatureResponse = await _httpClient.GetStringAsync(TemperatureURL);

                return Ok(new ApiResponseBody(true, null, new
                {
                    Temperature = Convert.ToDouble(temperatureResponse, CultureInfo.InvariantCulture)
                }));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseBody(false, e.Message));
            }
        }
    }
}
