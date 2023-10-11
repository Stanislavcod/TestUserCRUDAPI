using UserTestCRUD.BusinessLogic.Implementations;
using UserTestCRUD.BusinessLogic.Interfaces;
using UserTestCRUD.BusinessLogic.Validator;
using UserTestCRUD.DAL.Seed;

namespace UserTestCRUD.API.Configuration
{
    public static class ServiceExtensions
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddScoped<DataSeeder>();
            services.AddTransient<IUserValidator, UserValidator>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
        }
    }
}
