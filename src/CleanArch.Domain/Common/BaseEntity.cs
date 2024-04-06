using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        virtual public DateTime UpdatedDate { get; set; }
    }
}
