using UserTestCRUD.API.Middleware;
using UserTestCRUD.DAL.DataBaseContext;

namespace UserTestCRUD.API.Configuration
{
    public static class ConfigurationService
    {
        public static void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDatabase(configuration);
            services.AddControllers();
            services.AddCustomService();
            services.AddEndpointsApiExplorer();
            services.AddCustomSwagger();
            services.AddCustomLogging();
            services.AddCustomAutoMapper();
            services.AddCustomAuthentication(configuration);
        }

        public static void Configure(WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCustomExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.MapControllers();

            app.ConfigurateDataSeeder();
        }
    }
}
