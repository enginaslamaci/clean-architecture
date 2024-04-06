using CleanArch.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace CleanArch.Persistence.Contexts.Seeds.Identity
{
    public static class MappingUserRole
    {
        public static List<ApplicationUserRole> IdentityUserRoleList(List<ApplicationUser> users, List<ApplicationRole> roles)
        {
            var userRoles = new List<ApplicationUserRole>();

            for (int i = 0; i < roles.Count; i++)
            {
                userRoles.Add(new ApplicationUserRole
                {
                    RoleId = roles[i].Id,
                    UserId = users[i].Id
                });
            }

            return userRoles;
        }
    }
}
