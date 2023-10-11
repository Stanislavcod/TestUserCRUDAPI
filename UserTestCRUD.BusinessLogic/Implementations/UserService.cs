using Microsoft.EntityFrameworkCore;
using UserTestCRUD.BusinessLogic.Interfaces;
using UserTestCRUD.BusinessLogic.ViewModel;
using UserTestCRUD.DAL.DataBaseContext;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.BusinessLogic.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;

        public UserService(ApplicationDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int page, int pageSize, string SortBy, string filter)
        {
            var query = _context.Users.AsNoTracking().Include(u => u.Roles).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(u =>
                u.Name.Contains(filter) ||
                u.Email.Contains(filter) ||
                u.Age.ToString().Contains(filter) ||
                u.Roles.Any(r => r.Name.Contains(filter)));
            }

            switch (SortBy)
            {
                case "Name":
                    query = query.OrderBy(u => u.Name);
                    break;
                case "Age":
                    query = query.OrderBy(u => u.Age);
                    break;
                case "Email":
                    query = query.OrderBy(u => u.Email);
                    break;
                case "Role.Id":
                    query = query.OrderBy(u => u.Roles.OrderBy(r => r.Id).FirstOrDefault().Id);
                    break;
                case "Role.Name":
                    query = query.OrderBy(u => u.Roles.OrderBy(r => r.Name).FirstOrDefault().Name);
                    break;
                default:
                    query = query.OrderBy(u => u.Id);
                    break;
            }

            var totalItems = await query.CountAsync();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<User> CreateUserAsync(UserViewModel viewModel)
        {
            var existingRoleName = viewModel.Roles?.Select(r => r.Name).ToList();
            var existingRoles = await _roleService.GetExistingRolesAsync(existingRoleName);

            var user = new User
            {
                Name = viewModel.Name,
                Age = viewModel.Age,
                Email = viewModel.Email,
                Roles = existingRoles.ToList()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateUserAsync(int userId, UserViewModel viewModel)
        {
            var existingUser = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);

            existingUser.Name = viewModel.Name;
            existingUser.Email = viewModel.Email;
            existingUser.Age = viewModel.Age;

            existingUser.Roles.Clear();

            _context.Update(existingUser);

            if (viewModel.Roles != null && viewModel.Roles.Any())
            {
                var existingRole = await _context.Roles
                    .Where(r => viewModel.Roles.Select(x => x.Name).Contains(r.Name))
                    .ToListAsync();

                existingUser.Roles.AddRange(existingRole);
            }

            await _context.SaveChangesAsync();

            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddRoleToUserAsync(int userId, string roleName)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (user is null || role is null)
            {
                return false;
            }

            if (!user.Roles.Any(r => r.Id == role.Id))
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
