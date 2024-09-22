namespace Create.Api.Helpers.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        // Log da solicitação
        _logger.LogInformation($"Request: {httpContext.Request.Method} {httpContext.Request.Path}");

        // Captura o corpo da solicitação
        var requestBodyStream = new MemoryStream();
        var originalRequestBody = httpContext.Request.Body;
        await httpContext.Request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        var requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();
        _logger.LogInformation($"Request Body: {requestBodyText}");

        // Configura o corpo da solicitação para ser lido novamente no pipeline
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        httpContext.Request.Body = requestBodyStream;

        // Captura a resposta
        var originalResponseBodyStream = httpContext.Response.Body;
        var responseBodyStream = new MemoryStream();
        httpContext.Response.Body = responseBodyStream;

        await _next(httpContext);

        // Log da resposta
        _logger.LogInformation($"Response: {httpContext.Response.StatusCode}");
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(responseBodyStream).ReadToEndAsync();
        _logger.LogInformation($"Response Body: {responseBodyText}");

        // Configura o corpo da resposta para ser enviado ao cliente
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        httpContext.Response.Body = originalResponseBodyStream;

    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class LoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingApi(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}