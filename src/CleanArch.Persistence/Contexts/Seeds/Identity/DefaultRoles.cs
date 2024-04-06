using CleanArch.Application.Enums;
using CleanArch.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.Persistence.Contexts.Seeds.Identity
{
    public static class DefaultRoles
    {
        public static List<ApplicationRole> IdentityRoleList()
        {
            return new List<ApplicationRole>()
            {
                new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Roles.SuperAdmin.ToString(),
                    NormalizedName = Roles.SuperAdmin.ToString().ToUpperInvariant()
                },
                new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Roles.Admin.ToString(),
                    NormalizedName = Roles.Admin.ToString().ToUpperInvariant()
                },
                new ApplicationRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Roles.Basic.ToString(),
                    NormalizedName = Roles.Basic.ToString().ToUpperInvariant()
                }
            };
        }
    }
}
