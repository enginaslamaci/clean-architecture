using CleanArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<Customer>(x => x.Customer)
            .WithMany(s => s.Orders)
            .HasForeignKey(x => x.CustomerId);

            builder.HasMany<OrderItem>(x => x.OrderItems)
             .WithOne(s => s.Order)
             .HasForeignKey(x => x.OrderId);
        }
    }

}
