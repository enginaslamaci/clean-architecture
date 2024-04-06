using CleanArch.Application.Enums;
using CleanArch.Application.Exceptions;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Common.Results.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace CleanArch.Application.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly AppType _appType;
        public ExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger, AppType appType)
        {
            _next = next;
            _logger = logger;
            _appType = appType;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }
        }

        private async Task ConvertException(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = Result.Fail(exception.Message);

            switch (exception)
            {
                case ValidationException validationException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Messages = validationException.Errors;
                    break;
                case BadRequestException badRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ApiException apiException:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            string errorMessage = string.Concat("\n Exception: " + exception.Message +
                                                "\n Messages: " + string.Join(",", responseModel.Messages));
            _logger.LogError(errorMessage);

            switch (_appType)
            {
                case AppType.WebApi:
                    var result = JsonConvert.SerializeObject(responseModel);
                    await response.WriteAsync(result);
                    break;
                case AppType.WebApp:
                    context.Response.Redirect("/Home/Error");
                    break;
            }

        }
    }
}
