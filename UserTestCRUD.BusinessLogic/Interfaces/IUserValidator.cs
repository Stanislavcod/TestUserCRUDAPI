using FluentValidation;
using UserTestCRUD.BusinessLogic.ViewModel;

namespace UserTestCRUD.BusinessLogic.Interfaces
{
    public interface IUserValidator : IValidator<UserViewModel>
    {
    }
}
