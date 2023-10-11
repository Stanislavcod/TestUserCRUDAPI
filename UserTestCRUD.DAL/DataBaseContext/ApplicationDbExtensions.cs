using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserTestCRUD.DAL.DataBaseContext
{
    public static class ApplicationDbExtensions
    {
        public static void AddApplicationDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                opt => opt.MigrationsAssembly("UserTestCRUD.DAL")));
        }
    }
}
