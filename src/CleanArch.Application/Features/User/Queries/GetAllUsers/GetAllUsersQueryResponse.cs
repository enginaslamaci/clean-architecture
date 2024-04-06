using CleanArch.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Features.User.Queries.GetAllUsers
{
    public class GetAllUsersQueryResponse
    {
        public List<ApplicationUser> Users { get; set; }
    }
}
