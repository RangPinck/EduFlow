using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using EduFlowApi.DTOs.UserDTOs;
using EduFlowApi.Interfaces;
using EduFlowApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace EduFlowApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AuthUser> _userManager;

        public UserController(IUserRepository userRepository, UserManager<AuthUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [SwaggerOperation(Summary = "Получение всех пользователей")]
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var loginUser = HttpContext.User;
                var currentUser = _userManager.FindByEmailAsync(loginUser.Identity.Name).Result;
                var userRoles = _userManager.GetRolesAsync(currentUser).Result.ToList();

                var users = await _userRepository.GetAllUsersAsync(userRoles.Contains("Администратор"));

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                Log.Error($"get all users => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение данных авторизованного пользователя")]
        [HttpGet("GetAuthorizeUserData")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetAuthorizeUserData()
        {
            try
            {
                var loginUser = HttpContext.User;
                var currentUser = _userManager.FindByEmailAsync(loginUser.Identity.Name).Result;
                var userRoles = _userManager.GetRolesAsync(currentUser).Result.ToList();

                var user = await _userRepository.GetLogginedUserWithStatisticsAsync(currentUser.Id);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error($"get authorize user data => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Получение данных конкретного пользователя по ID")]
        [HttpGet("GetUserDataById")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Администратор, Куратор")]
        public async Task<IActionResult> GetUserDataById([FromQuery] Guid userId)
        {
            try
            {
                var loginUser = HttpContext.User;
                var currentUser = _userManager.FindByEmailAsync(loginUser.Identity.Name).Result;
                var userRoles = _userManager.GetRolesAsync(currentUser).Result.ToList();

                var user = await _userRepository.GetLogginedUserWithStatisticsAsync(userId);

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error($"get authorize user data => {ex.Message}");
                return StatusCode(503, ex.Message);
            }
        }
    }
}
