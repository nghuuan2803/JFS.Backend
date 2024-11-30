using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JFS.Backend.DTOs;
using JFS.Backend.Enitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace JFS.Backend.Services
{
    public class AuthService(IConfiguration _configuration)
    {
        List<User> userData = MockUserData.Users;
        public async Task<LoginResult> LoginAsync(LoginDTO user)
        {
            User authUser = userData.FirstOrDefault(p => p.UserName == user.UserName);
            if (authUser == null)
            {
                return new LoginResult(false, "Tài khoản không tồn tại", "");
            }
            if (authUser.Password != user.Password)
            {
                return new LoginResult(false, "Mật khẩu không chính xác", "");
            }

            string token = await GenerateToken(authUser);
            return new LoginResult(true, "Đăng nhập thành công", token);
        }

        private async Task<string> GenerateToken(User user)
        {
            string role = user.Role;

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role,role),
                new Claim("student","Sv HUFLIT"),
            };

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var secureToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:Expire"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(secureToken);
        }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }

        public LoginResult(bool success, string? errorMessage, string? token)
        {
            Success = success;
            Token = token;
            ErrorMessage = errorMessage;
        }
    }
}
