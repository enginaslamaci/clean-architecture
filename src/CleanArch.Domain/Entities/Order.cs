using CleanArch.Domain.Common;

namespace CleanArch.Domain.Entities
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int TotalAmount { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
