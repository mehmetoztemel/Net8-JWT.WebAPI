namespace Net8_JWT.WebAPI.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseMyCustomMiddlewares(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogMiddleware>();
        }
    }
}
