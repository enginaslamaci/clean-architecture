using CleanArch.Application.DTOs.User;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Features.User.Queries.GetUserByRole
{
    public class GetUserByRoleQueryResponse
    {
        public PaginatedResult<ApplicationUserDto> Users { get; set; }
    }
}
