using System.Text.Json.Serialization;

namespace MagPie_Home_Control_API.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PowerStates { On, Off }
}
