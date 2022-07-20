namespace WebAPIAutores.Middleware
{
    public static class LoguearRespuestaHTTPMiddlewareExtension
    {
        public static IApplicationBuilder UseLoguearRespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
        }
    }

    public class LoguearRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoguearRespuestaHTTPMiddleware> logger;

        public LoguearRespuestaHTTPMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        //Invoke o InvokeAsync
        public async Task InvokeAsync(HttpContext context)
        {
            using (var memoryS = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = context.Response.Body;
                context.Response.Body = memoryS;

                await siguiente(context);
                memoryS.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(memoryS).ReadToEnd();
                await memoryS.CopyToAsync(cuerpoOriginalRespuesta);
                context.Response.Body = cuerpoOriginalRespuesta;

                logger.LogInformation(respuesta);
            }
        }
    }
}
