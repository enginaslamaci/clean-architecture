using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Abstractions.Infrastructure.Services
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        bool IsAuthenticated { get; }

        List<KeyValuePair<string, string>> Claims { get; }
    }
}
