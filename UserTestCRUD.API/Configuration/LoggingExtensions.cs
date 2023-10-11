using Serilog;
using Serilog.Events;

namespace UserTestCRUD.API.Configuration
{
    public static class LoggingExtensions
    {
        public static void AddCustomLogging(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"logs/log_{DateTime.Now:yyyyMMdd}.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Information() 
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc => lc
                        .Filter.ByExcluding(e => e.Level == LogEventLevel.Debug)
                        .WriteTo.Console()
                                )
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
            });
        }
    }
}
