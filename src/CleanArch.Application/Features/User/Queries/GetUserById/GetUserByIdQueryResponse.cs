using CleanArch.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Features.User.Queries.GetUserById
{
    public class GetUserByIdQueryResponse
    {
        public ApplicationUserDto User { get; set; }
    }
}
