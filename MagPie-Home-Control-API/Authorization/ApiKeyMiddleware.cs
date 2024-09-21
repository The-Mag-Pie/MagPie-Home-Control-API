using MagPie_Home_Control_API.Models;
using System.Text.Json;

namespace MagPie_Home_Control_API.Authorization
{
    // Temporary solution for endpoint security
    public class ApiKeyMiddleware : IMiddleware
    {
        private readonly string _apiKey;

        public ApiKeyMiddleware(IConfiguration configuration)
        {
            _apiKey = configuration["API_KEY"] ?? throw new EnvironmentVariableMissingException("API_KEY");
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");

            // preflight
            if (context.Request.Method == HttpMethods.Options)
            {
                context.Response.StatusCode = 204;
                context.Response.Headers.Append("Access-Control-Allow-Headers", "X-Api-Key");
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/problem+json; charset=utf-8";

                var response = new ApiResponseBody(false, "API key not found");

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            var apiKey = extractedApiKey.ToString();
            if (apiKey != _apiKey)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/problem+json; charset=utf-8";

                var response = new ApiResponseBody(false, "Invalid API key");

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            await next(context);
        }
    }
}
