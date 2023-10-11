using UserTestCRUD.DAL.Seed;
using UserTestCRUD.DAL.DataBaseContext;

namespace UserTestCRUD.API.Configuration
{
    public static class DataSeederExtensions
    {
        public static void ConfigurateDataSeeder(this IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                var dataSeeder = new DataSeeder(context);

                dataSeeder.Initialize();
            }
        }
    }
}
