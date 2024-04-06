using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.DTOs.User
{
    public class ApplicationUserRoleDto
    {
        public virtual ApplicationUserDto User { get; set; }
        public virtual ApplicationRoleDto Role { get; set; }
    }
}
