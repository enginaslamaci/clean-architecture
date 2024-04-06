using CleanArch.Domain.Common;

namespace CleanArch.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Package { get; set; }
        public bool IsDiscontinued { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
