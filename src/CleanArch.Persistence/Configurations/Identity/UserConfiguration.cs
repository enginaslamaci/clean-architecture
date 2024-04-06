using CleanArch.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Persistence.Configurations.Identity
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Primary key
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName).HasMaxLength(256);
            builder.Property(u => u.LastName).HasMaxLength(256);
            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Property(u => u.Email).HasMaxLength(256); 
            builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
            builder.Property(u => u.PasswordHash).HasMaxLength(450);
            builder.Property(u => u.SecurityStamp).HasMaxLength(256);
            builder.Property(u => u.ConcurrencyStamp).HasMaxLength(256);
            builder.Property(u => u.PhoneNumber).HasMaxLength(50);

        }
    }
}
