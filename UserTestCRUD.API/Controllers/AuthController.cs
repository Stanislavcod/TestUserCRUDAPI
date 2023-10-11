using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using UserTestCRUD.API.Constants;
using UserTestCRUD.API.Contracts;
using UserTestCRUD.BusinessLogic.Interfaces;
using UserTestCRUD.BusinessLogic.ViewModel;
using UserTestCRUD.DAL.Entities;
using UserTestCRUD.DAL.Result;

namespace UserTestCRUD.API.Controllers
{
    /// <summary>
    /// Контроллер аутентификации и авторизации пользователей.
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Метод для аутентификации пользователя.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /auth/login
        ///     {
        ///        "email": "stas.dryk@gmail.com"
        ///     }
        ///
        /// </remarks>
        /// <param name="userLoginViewModel">Модель данных для входа пользователя.</param>
        /// <returns>Аутентифицированный пользователь или код ошибки.</returns>
        [HttpPost(ApiRoutes.Auth.Login)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerOperation("AuthUser")]
        public async Task<IActionResult> Get(UserLoginViewModel userLoginViewModel)
        {
            try
            {
                var user = await _authService.AuthenticateAsync(userLoginViewModel);

                _logger.LogInformation($"User with email '{userLoginViewModel.Email}' authenticated successfully.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AuthenticateAsync method.");
                return BadRequest(ex.Message);
            }
        }
    }
}
