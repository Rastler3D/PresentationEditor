using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PresentationEditor.Server.Models
{
    public class User
    {
        public UserID Id { get; set; }
        public string Name { get; set; }

        public User(ClaimsPrincipal user)
        {
            var user1 = user;
            Id = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            Name = user.FindFirstValue(JwtRegisteredClaimNames.Name);
        }
    }

   
}
