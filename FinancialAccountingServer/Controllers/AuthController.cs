using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAccountingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserDTO userDto)
        {
            if (await _authService.RegisterUser(userDto))
            {
                return Ok("Registered");
            }

            return BadRequest("Smth went wrong during registration");

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            if (await _authService.Login(userDTO))
            {
                var tokenString = await _authService.GenerateAccessToken(userDTO);

                var refreshToken = await _authService.GenerateRefreshToken(userDTO);

                var tokenResponseDTO = new TokenResponseDTO
                {
                    AccessToken = tokenString,
                    RefreshToken = refreshToken
                };

                return Ok(tokenResponseDTO);
            }

            return BadRequest();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto refreshToken)
        {
            var response = await _authService.RefreshAccessToken(refreshToken);

            if (response != null)
            {
                return Ok(response);
            }

            return Unauthorized("Token is not valid");
        }

        [HttpGet("get-user-name/{userId}")]
        public async Task<IActionResult> GetUserName(int userId)
        {
            var response = await _authService.GetUserName(userId);
            return Ok(response);
        }

        [HttpGet("get-user-info/{userId}")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var response = await _authService.GetUserInfo(userId);
            return Ok(response);
        }

        [HttpPost("add-avatar/{userId}")]
        public async Task<IActionResult> AddAvatarToUser(int userId, [FromBody] string imagePath)
        {
            var response = await _authService.AddAvatarToUser(userId, imagePath);
            return Ok(response);
        }

        [HttpGet("get-user-avatar/{userId}")]
        public async Task<IActionResult> GetUserAvatar(int userId)
        {
            var response = await _authService.GetAvatarOfUser(userId);
            return Ok(response);
        }

        [HttpDelete("delete-user-avatar/{userId}")]
        public async Task<IActionResult> DeleteUserAvatar(int userId)
        {
            var response = await _authService.DeleteAvatarOfUser(userId);
            return Ok(response);
        }
    }
}
