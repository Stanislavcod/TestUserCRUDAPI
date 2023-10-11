using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.BusinessLogic.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetExistingRolesAsync(IEnumerable<string> roleName);
    }
}
