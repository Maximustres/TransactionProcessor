using System.Net;

namespace api_transaction.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER_NAME = "X-API-KEY";
        private readonly string _expectedApiKey;
        private readonly ILogger<ApiKeyMiddleware> _logger;

        public ApiKeyMiddleware(
            RequestDelegate next, 
            IConfiguration configuration, 
            ILogger<ApiKeyMiddleware> logger)
        {
            _next = next;
            _expectedApiKey = configuration.GetValue<string>("ApiSettings:ApiKey");
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                _logger.LogWarning("Missing API key");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            if (!_expectedApiKey.Equals(extractedApiKey))
            {
                _logger.LogWarning("Invalid API key: {ExtractedApiKey}", extractedApiKey.ToString());
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            await _next(context);
        }

    }
}
