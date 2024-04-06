using CleanArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.City).HasMaxLength(50);
            builder.Property(x => x.Country).HasMaxLength(50);
            builder.Property(x => x.Phone).HasMaxLength(15);

            builder.HasMany<Order>(x => x.Orders)
            .WithOne(s => s.Customer)
            .HasForeignKey(x => x.CustomerId);
        }
    }

}
