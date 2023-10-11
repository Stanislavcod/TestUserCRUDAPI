using Microsoft.EntityFrameworkCore;
using UserTestCRUD.BusinessLogic.Interfaces;
using UserTestCRUD.DAL.DataBaseContext;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.BusinessLogic.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetExistingRolesAsync(IEnumerable<string> roleName)
        {
            return await _context.Roles
                .Where(r => roleName.Contains(r.Name))
                .ToListAsync();
        }
    }
}
