using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using UserTestCRUD.API.Constants;
using UserTestCRUD.API.Contracts;
using UserTestCRUD.BusinessLogic.Interfaces;
using UserTestCRUD.BusinessLogic.ViewModel;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserValidator _userValidator;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IUserValidator userValidator, ILogger<UserController> logger)
        {
            _userService = userService;
            _userValidator = userValidator;
            _logger = logger;
        }

        /// <summary>
        /// Получение списка пользователей.
        /// </summary>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <param name="sortBy">Поле для сортировки.</param>
        /// <param name="filter">Фильтр (необязательно).</param>
        /// <returns>Список пользователей.</returns>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Users.GetUsers)]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [SwaggerOperation("GetAllUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1,
            [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id",
            [FromQuery] string? filter = null)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(page, pageSize, sortBy, filter);
                _logger.LogInformation("Retrieved users successfully.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving users.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Получение пользователя по ID.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Пользователь или код 404, если не найден.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet(ApiRoutes.Users.GetUserById)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("GetUserById")]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user is null)
                {
                    _logger.LogWarning($"User with ID {userId} not found.");
                    return NotFound();
                }
                _logger.LogInformation($"Retrieved user with ID {userId} successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retrieving user with ID {userId}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /users
        ///     {
        ///        "name": "stas",
        ///        "age": 21,
        ///        "email": "stas.dryk@gmail.com",
        ///        "roles": [
        ///          {
        ///             "name": "Admin"
        ///          }
        ///      ]
        ///     }
        ///
        /// </remarks>
        /// <param name="viewModel">Данные пользователя.</param>
        /// <returns>Созданный пользователь или код ошибки.</returns>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Users.CreateUser)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerOperation("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel viewModel)
        {
            try
            {
                var validationResult = await _userValidator.ValidateAsync(viewModel);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for user creation.");
                    return BadRequest(validationResult.Errors);
                }

                var createdUser = await _userService.CreateUserAsync(viewModel);

                if (createdUser is null)
                {
                    _logger.LogWarning("Failed to create user.");
                    return BadRequest("Failed to create user");
                }

                _logger.LogInformation($"User with ID {createdUser.Id} created successfully.");
                return CreatedAtAction(nameof(GetUser), new { userId = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Обновление существующего пользователя.
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     PUT /users/{userId:int}
        ///     {
        ///        "name": "stas",
        ///        "age": 21,
        ///        "email": "stas.dryk@gmail.com",
        ///        "roles": [
        ///          {
        ///             "name": "Admin"
        ///          }
        ///      ]
        ///     }
        ///
        /// </remarks>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="viewModel">Данные для обновления.</param>
        /// <returns>Обновленный пользователь или код ошибки.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut(ApiRoutes.Users.UpdateUser)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserViewModel viewModel)
        {
            try
            {
                var validationResult = await _userValidator.ValidateAsync(viewModel);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning($"Validation failed for user update (ID: {userId}).");
                    return BadRequest(validationResult.Errors);
                }

                var updatedUser = await _userService.UpdateUserAsync(userId, viewModel);

                if (updatedUser is null)
                {
                    _logger.LogWarning($"User with ID {userId} not found for update.");
                    return NotFound();
                }

                _logger.LogInformation($"User with ID {userId} updated successfully.");
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating user with ID {userId}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Удаление пользователя по ID.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Код 204 в случае успеха или код ошибки.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete(ApiRoutes.Users.RemoveUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerOperation("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var user = await _userService.DeleteUserAsync(userId);

                if (user)
                {
                    _logger.LogInformation($"User with ID {userId} deleted successfully.");
                    return NoContent();
                }

                _logger.LogWarning($"User with ID {userId} not found, unable to delete.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteUser method for user with ID {userId}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Добавление роли пользователю.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="roleName">Наименование роли.</param>
        /// <returns>Код 200 в случае успеха или код ошибки.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Users.AddRoleToUser)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerOperation("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser(int userId, string roleName)
        {
            try
            {
                var added = await _userService.AddRoleToUserAsync(userId, roleName);

                if (added)
                {
                    _logger.LogInformation($"Role '{roleName}' added to user with ID {userId}.");
                    return Ok();
                }

                _logger.LogWarning($"User with ID {userId} not found, unable to add role '{roleName}'.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in AddRoleToUser method for user with ID {userId} and role '{roleName}'.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred.");
            }
        }
    }
}
