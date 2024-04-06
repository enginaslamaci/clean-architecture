using CleanArch.Domain.Entities.Identity;
using CleanArch.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Persistence.Contexts.Seeds.Identity
{
    public static class IdentityContextSeed
    {
        public static void IdentitySeed(this ModelBuilder modelBuilder)
        {
            var roles = CreateRoles(modelBuilder);
            var users = CreateBasicUsers(modelBuilder);
            MapUserRole(modelBuilder, users, roles);
        }

        private static List<ApplicationRole> CreateRoles(ModelBuilder modelBuilder)
        {
            List<ApplicationRole> roles = DefaultRoles.IdentityRoleList();
            modelBuilder.Entity<ApplicationRole>().HasData(roles);
            return roles;
        }

        private static List<ApplicationUser> CreateBasicUsers(ModelBuilder modelBuilder)
        {
            List<ApplicationUser> users = DefaultUser.IdentityBasicUserList();
            modelBuilder.Entity<ApplicationUser>().HasData(users);
            return users;
        }

        private static void MapUserRole(ModelBuilder modelBuilder, List<ApplicationUser> users, List<ApplicationRole> roles)
        {
            var identityUserRoles = MappingUserRole.IdentityUserRoleList(users, roles);
            modelBuilder.Entity<ApplicationUserRole>().HasData(identityUserRoles);
        }
    }
}
