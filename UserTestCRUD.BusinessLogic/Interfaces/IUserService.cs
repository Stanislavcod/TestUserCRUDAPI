using UserTestCRUD.BusinessLogic.ViewModel;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync(int page, int pageSize, string sortBy, string filter);
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(UserViewModel createdUserViewModel);
        Task<User> UpdateUserAsync(int id, UserViewModel updatedUserViewModel);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> AddRoleToUserAsync(int userId, string roleName);
    }
}
