using System.Text.Json;

namespace MagPie_Home_Control_API.Models
{
    public class ApiResponseBody(bool success, string? message = null, object? data = null)
    {
        public bool Success { get; set; } = success;
        public string? Message { get; set; } = message;
        public object? Data { get; set; } = data;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
