using CleanArch.Application.Abstractions.Infrastructure.Services.Token;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Settings;
using CleanArch.Infrastructure.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace CleanArch.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly UserManager<Domain.Entities.Identity.ApplicationUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        public TokenHandler(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> CreateAccessToken(ApplicationUser appUser)
        {
            var userClaims = await _userManager.GetClaimsAsync(appUser);
            var roles = await _userManager.GetRolesAsync(appUser);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
                {
                // The username using the Identity name so it fills out the HttpContext.User.Identity.Name value
                new Claim(ClaimsIdentity.DefaultNameClaimType, appUser.UserName),

                // Add user Id so that UserManager.GetUserAsync can find the user based on Id
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim("uid", appUser.Id),

                new Claim("ip", ipAddress)
            }
                .Union(userClaims)
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public async Task<RefreshToken> CreateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            byte[] number = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
