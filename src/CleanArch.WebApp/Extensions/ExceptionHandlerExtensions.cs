using CleanArch.Domain.Common.Results;

namespace CleanArch.WebApp.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static async Task<string> ErrorResult(this Refit.ApiException exception)
        {
            var result = await exception.GetContentAsAsync<Result>();
            return result?.Messages != null ? string.Join("; ", result.Messages) : "Something went wrong";
        }
    }
}
