using MagPie_Home_Control_API.Enums;

namespace MagPie_Home_Control_API.Models
{
    public class ToggleResponse(PowerStates oldState, PowerStates newState) : 
        ApiResponseBody(true, "Toggle operation completed successfully.", new
        {
            OldState = oldState,
            NewState = newState
        })
    {
    }
}
