using FinancialAccountingServer.Data;
using FinancialAccountingServer.DTOs;
using FinancialAccountingServer.models;
using FinancialAccountingServer.repositories.interfaces;
using FinancialAccountingServer.Services.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinancialAccountingServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly FinancialAppContext _dbContext;
        private readonly IBlobService _blobService;

        public AuthService(IUserRepository userRepository, IConfiguration config, IRefreshTokenRepository refreshTokenRepository, FinancialAppContext dbContext, IBlobService blobService) 
        {
            _userRepository = userRepository;
            _config = config;
            _refreshTokenRepository = refreshTokenRepository;
            _dbContext = dbContext;
            _blobService = blobService;
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public async Task<bool> Login(UserDTO userDTO)
        {
            var user = await _userRepository.GetUserByUsername(userDTO.Username);

            if (user == null) 
            {
                return false;
            }

            string hashedPassword = HashPassword(userDTO.Password, user.PasswordSalt);

            return user.PasswordHash == hashedPassword;
        }

        public async Task<bool> RegisterUser(UserDTO userDTO)
        {
            if (await _userRepository.CheckUsernameExists(userDTO.Username))
            {
                return false;
            }

            string passwordSalt = GenerateSalt();

            string passwordHash = HashPassword(userDTO.Password, passwordSalt);

            User newUser = new User
            {
                Username = userDTO.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.AddUser(newUser);

            return true;
        }

        public async Task<string> GenerateAccessToken(UserDTO userDTO)
        {
            var userRole = _userRepository.GetRoleByUserName(userDTO.Username);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, userDTO.Username),
                new(ClaimTypes.Role, userRole.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            SigningCredentials signinCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signinCred
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;
        }

        public async Task<RefreshToken> GenerateRefreshToken(UserDTO userDto)
        {
            var user = await _userRepository.GetUserByUsername(userDto.Username);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMonths(1),
                Created = DateTime.Now,
                UserId = user.Id
            };

            await _refreshTokenRepository.RemoveRefreshToken(refreshToken.UserId);

            await _refreshTokenRepository.AddRefreshToken(refreshToken);

            return refreshToken;
        }

        public async Task<string> RefreshAccessToken(RefreshTokenDto refreshToken)
        {
            var savedToken = await _refreshTokenRepository.GetRefreshTokenByUserId(refreshToken.UserId);

            if (savedToken.Token != refreshToken.Token)
                return null;

            if (savedToken.Expires < DateTime.Now)
                return null;

            var iuser = await _userRepository.GetUserById(refreshToken.UserId);

            var user = new UserDTO
            {
                Username = iuser.Username,
                Password = ""
            };

            return await GenerateAccessToken(user);
        }

        public async Task<string> GetUserName(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return user.Username;
        }

        public async Task<bool> AddAvatarToUser(int userId, string imagePath)
        {
            User user = await _userRepository.GetUserById(userId);

            user.AvatarPath = imagePath;

            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _dbContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<string> GetAvatarOfUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return user?.AvatarPath;
        }

        public async Task<bool> DeleteAvatarOfUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            var avatarPath = user.AvatarPath;

            if(avatarPath == null)
            {
                return false;
            }

            await _blobService.DeleteBlobAsync(avatarPath);

            user.AvatarPath = null;

            return await Save();
        }
    }
}
