using MagPie_Home_Control_API.Enums;
using MagPie_Home_Control_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagPie_Home_Control_API.Controllers.CentralHeating.Stove
{
    //[Route("[controller]")]
    [Route("centralheating/stove/state")]
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
                    return Ok(new StateResponse(PowerStates.On));
                }
                else if (ewelinkResponse == "off")
                {
                    return Ok(new StateResponse(PowerStates.Off));
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

        // TODO: Implement POST method to change state instead of another controller
    }
}
