using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserTestCRUD.BusinessLogic.Interfaces;
using UserTestCRUD.BusinessLogic.ViewModel;
using UserTestCRUD.DAL.DataBaseContext;

namespace UserTestCRUD.BusinessLogic.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthService(ITokenService tokenService, ApplicationDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<SuccessLoginViewModel> AuthenticateAsync(UserLoginViewModel userLoginViewModel)
        {
            var user = await _context.Users.Include(r=> r.Roles)
                .FirstOrDefaultAsync(u => u.Email == userLoginViewModel.Email);

            if (user == null)
            {
                throw new InvalidOperationException($"User with login - {userLoginViewModel.Email} not found.");
            }

            var token = _tokenService.GetToken(user);
            var userViewModel =  _mapper.Map<UserViewModel>(user);
            var succesLoginDto = new SuccessLoginViewModel
            {
                Token = token,
                UserViewModel = userViewModel,
            };

            return succesLoginDto;
        }
    }
}
