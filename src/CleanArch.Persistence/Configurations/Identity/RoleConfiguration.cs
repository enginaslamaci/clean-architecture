using CleanArch.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Persistence.Configurations.Identity
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            // Primary key
            builder.HasKey(r => r.Id);

            builder.Property(u => u.Name).HasMaxLength(256);
            builder.Property(u => u.NormalizedName).HasMaxLength(256);
            builder.Property(u => u.ConcurrencyStamp).HasMaxLength(450);

            builder
                  .HasMany(u => u.UserRoles)
                  .WithOne(r => r.Role)
                  .HasForeignKey(u => u.RoleId)
                  .IsRequired();

        }
    }
}