using MagPie_Home_Control_API.Enums;

namespace MagPie_Home_Control_API.Models
{
    public class StateResponse(PowerStates state) : ApiResponseBody(true, null, new { state })
    {
    }
}
