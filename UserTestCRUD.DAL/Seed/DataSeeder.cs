using UserTestCRUD.DAL.DataBaseContext;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.DAL.Seed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            SeedUsers();
            SeedRoles();
            _context.SaveChanges();
        }

        public void SeedRoles()
        {
            if(!_context.Roles.Any())
            {
                var roles = new List<Role>()
                {
                    new Role {Name = "User"},
                    new Role {Name = "Admin"},
                    new Role {Name = "Support"},
                    new Role {Name = "SuperAdmin"}
                };
                _context.Roles.AddRange(roles);
            }
        }

        public void SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var users = new List<User>()
                {
                    new User() { Name = "Иван", Age = 14, Email = "ivan.ivan@gmail.com" },
                    new User() { Name = "Стас", Age = 21, Email = "stas.dryk@gmail.com" },
                    new User() { Name = "Никита", Age = 21, Email = "nikita@gmail.com" }
                };
                _context.Users.AddRange(users);
            }
        }
    }
}
