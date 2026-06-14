namespace LibraryApi
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/auth"))
            {
                await _next(context);
                return;
            }
            var apiKey =
                Environment.GetEnvironmentVariable(
                    "LIBRARY_API_KEY");

            if (!context.Request.Headers.TryGetValue(
                    "X-API-KEY",
                    out var providedKey))
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsync(
                    "API Key Missing");

                return;
            }

            if (apiKey != providedKey)
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsync(
                    "Invalid API Key");

                return;
            }

            await _next(context);
        }
    }
}
