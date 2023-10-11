namespace UserTestCRUD.API.Middleware
{
    public static class ExceptionMiddleware
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}
