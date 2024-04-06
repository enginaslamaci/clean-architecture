using CleanArch.Application.Abstractions.Infrastructure.Services;

namespace CleanArch.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
