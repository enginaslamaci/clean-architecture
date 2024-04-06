using CleanArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Persistence.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CompanyName).HasMaxLength(50);
            builder.Property(x => x.ContactName).HasMaxLength(50);
            builder.Property(x => x.City).HasMaxLength(50);
            builder.Property(x => x.Country).HasMaxLength(50);
            builder.Property(x => x.Phone).HasMaxLength(15);
            builder.Property(x => x.Fax).HasMaxLength(15);

            builder.HasMany<Product>(x => x.Products)
                .WithOne(s => s.Supplier)
                .HasForeignKey(x => x.SupplierId);
        }
    }
}
