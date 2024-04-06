using CleanArch.Application.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Abstractions.Infrastructure.Services
{
    public interface IMailService
    {
        Task SendAsync(EmailRequest request);

        Task SendPasswordResetMailAsync(string to, string userId, string resetToken);
    }
}
