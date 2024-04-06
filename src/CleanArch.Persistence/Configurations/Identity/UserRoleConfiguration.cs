using CleanArch.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Persistence.Configurations.Identity
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            // key
            builder.HasKey(r => new { r.UserId, r.RoleId });


            builder
                  .HasOne(u => u.User)
                  .WithMany(r => r.Roles)
                  .HasForeignKey(u => u.UserId)
                  .IsRequired();

            builder
                .HasOne(u => u.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

        }
    }
}
