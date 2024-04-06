using CleanArch.Domain.Entities.Identity;

namespace CleanArch.Persistence.Contexts.Seeds.Identity
{
    public static class DefaultUser
    {
        public static List<ApplicationUser> IdentityBasicUserList()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                    FirstName = "Super",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    //password: 123456
                    PasswordHash = "AQAAAAIAAYagAAAAEN7jQCcH1BGePQOwz4Y0GnfRaB2aIig66Xp7T4ssPqqku/GW/8M82bA2re0ybfYTxA==",
                    NormalizedEmail= "SUPERADMIN@GMAIL.COM",
                    NormalizedUserName="SUPERADMIN"
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "adminuser",
                    Email = "adminuser@gmail.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    //password: 123456
                    PasswordHash = "AQAAAAIAAYagAAAAEN7jQCcH1BGePQOwz4Y0GnfRaB2aIig66Xp7T4ssPqqku/GW/8M82bA2re0ybfYTxA==",
                    NormalizedEmail= "ADMINUSER@GMAIL.COM",
                    NormalizedUserName="ADMINUSER"
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "basicuser",
                    Email = "basicuser@gmail.com",
                    FirstName = "Basic",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    //password: 123456
                    PasswordHash = "AQAAAAIAAYagAAAAEN7jQCcH1BGePQOwz4Y0GnfRaB2aIig66Xp7T4ssPqqku/GW/8M82bA2re0ybfYTxA==",
                    NormalizedEmail= "BASICUSER@GMAIL.COM",
                    NormalizedUserName="BASICUSER"
                }
            };
        }
    }
}
