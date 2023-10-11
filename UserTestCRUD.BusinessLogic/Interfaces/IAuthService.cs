using UserTestCRUD.BusinessLogic.ViewModel;

namespace UserTestCRUD.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<SuccessLoginViewModel> AuthenticateAsync(UserLoginViewModel userLoginViewModel);
    }
}
