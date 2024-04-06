using System.Security.Claims;

namespace CleanArch.Application.DTOs.Account.Request
{
    public class ExternalAuthDto
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public UserClaimsDto Claims { get; set; }

    }

    public class UserClaimsDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
