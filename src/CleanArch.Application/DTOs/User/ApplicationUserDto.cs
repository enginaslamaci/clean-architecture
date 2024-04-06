using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.DTOs.User
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get { return FirstName + " " + LastName; } }
        public List<ApplicationUserRoleDto> Roles { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string CreatedDate { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
