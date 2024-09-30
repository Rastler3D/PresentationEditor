using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserManagementService.Server.Utils
{
    public class JwtTokenGenerator {
        string secretKey;
        
        public JwtTokenGenerator(IConfiguration configuration)
        {
            this.secretKey = configuration["JwtSettings:Key"];
        }

        public string GenerateToken(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.Default.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Name, userName),
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
