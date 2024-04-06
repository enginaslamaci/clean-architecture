using CleanArch.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Persistence.Configurations.Identity
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Primary key
            builder.HasKey(r => r.Id);

            builder.Property(u => u.Token).HasMaxLength(256);
            builder.Property(u => u.CreatedByIp).HasMaxLength(256);
            builder.Property(u => u.RevokedByIp).HasMaxLength(256).IsRequired(false);
            builder.Property(u => u.Revoked).HasMaxLength(256).IsRequired(false);
            builder.Property(u => u.ReplacedByToken).HasMaxLength(256).IsRequired(false);
        }
    }
}
