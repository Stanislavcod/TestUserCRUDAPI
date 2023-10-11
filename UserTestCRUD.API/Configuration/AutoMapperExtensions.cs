using UserTestCRUD.BusinessLogic.Mapper;

namespace UserTestCRUD.API.Configuration
{
    public static class AutoMapperExtensions
    {
        public static void AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        }
    }
}
