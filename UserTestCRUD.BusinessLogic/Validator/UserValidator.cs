using FluentValidation;
using UserTestCRUD.BusinessLogic.Interfaces;
using UserTestCRUD.BusinessLogic.ViewModel;
using UserTestCRUD.DAL.DataBaseContext;

namespace UserTestCRUD.BusinessLogic.Validator
{
    public class UserValidator : AbstractValidator<UserViewModel>, IUserValidator
    {
        private readonly ApplicationDbContext _context;
        public UserValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Имя обязательно для заполнения.")
                .MaximumLength(50).WithMessage("Имя не должно превышать 50 символов.");

            RuleFor(user => user.Age)
                .NotEmpty().WithMessage("Возраст обязателен для заполнения.")
                .GreaterThan(0).WithMessage("Возраст должен быть положительным числом.");


            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email обязателен для заполнения.")
                .EmailAddress().WithMessage("Введите корректный адрес электронной почты.")
                .Must(BeUniqueEmail).WithMessage("Пользователь с такой почтой уже зарегестрирован в системе");
        }

        private bool BeUniqueEmail(string email)
        {
            return !_context.Users.Any(u => u.Email == email);
        }
    }
}
