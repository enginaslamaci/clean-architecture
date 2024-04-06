using CleanArch.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Common
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ApplicationUser UpdatedUser { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
    }
}
