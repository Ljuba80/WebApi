namespace ApiService.Api.ExceptionHandler;

public static class Extensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandler>();
    }
}
