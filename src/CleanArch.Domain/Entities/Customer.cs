using CleanArch.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace CleanArch.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
