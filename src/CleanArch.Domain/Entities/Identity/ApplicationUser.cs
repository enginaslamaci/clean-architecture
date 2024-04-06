using CleanArch.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public StatusType Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual List<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<ApplicationUserRole> Roles { get; set; }
    }
}
