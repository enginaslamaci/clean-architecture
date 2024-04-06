using CleanArch.Application.Abstractions.Infrastructure.Services;
using CleanArch.Application.Abstractions.Infrastructure.Services.Storage;
using CleanArch.Application.Abstractions.Infrastructure.Services.Token;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Settings;
using CleanArch.Infrastructure.Enums;
using CleanArch.Infrastructure.Services;
using CleanArch.Infrastructure.Services.Storage.AWS;
using CleanArch.Infrastructure.Services.Storage.Azure;
using CleanArch.Infrastructure.Services.Storage.Local;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanArch.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services

            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ITokenHandler, Services.Token.TokenHandler>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ICsvExporter, CsvExporter>();

            #endregion

            #region Configurations

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings")); //services.AddOptions<JWTSettings>().BindConfiguration("JWTSettings");

            #endregion

            #region Identity

            //JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();

                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                            var authToken = "";
                            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                                authToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");

                            if (string.IsNullOrEmpty(authToken))
                                return context.Response.WriteAsJsonAsync(Result.Fail("Token not found"));

                            var result = "Token validation has failed. Request access denied";
                            return context.Response.WriteAsJsonAsync(Result.Fail(result));

                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = "403 Not authorized";
                            return context.Response.WriteAsJsonAsync(Result.Fail(result));
                        },
                    };
                });

            #endregion

            #region Authorization

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("IsUser", policy => policy.RequireAuthenticatedUser());
            //    options.AddPolicy("CustomerCreate", policy => policy.RequireClaim("Customer.Create"));
            //});

            #endregion

        }

        //Storage
        public static void AddStorage(this IServiceCollection services, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    services.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    services.AddScoped<IStorage, AWSStorage>();
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }

    }
}
