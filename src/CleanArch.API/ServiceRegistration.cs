using Asp.Versioning;
using Azure;
using CleanArch.Domain.Common.Results;
using CleanArch.Domain.Settings;
using CleanArch.Persistence.Contexts;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Threading.RateLimiting;

namespace CleanArch.API
{
    public static class ServiceRegistration
    {
        public static void AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region configuration

            services.Configure<CORSSettings>(configuration.GetSection("CORS"));

            #endregion

            #region versioning

            var apiVersioningBuilder = services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                //o.ApiVersionReader = ApiVersionReader.Combine(
                //    new QueryStringApiVersionReader("api-version"),
                //    new HeaderApiVersionReader("X-Version"),
                //    new MediaTypeApiVersionReader("ver"));
            });

            //apiVersioningBuilder.AddApiExplorer(
            //                options =>
            //                {
            //                    options.GroupNameFormat = "'v'VVV";
            //                    options.SubstituteApiVersionInUrl = true;
            //                });

            #endregion

            #region swagger

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "CleanArchitecture", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
                        }
                    });
            });

            #endregion

            #region rate-limit

            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("Fixed", opt =>
                {
                    opt.Window = TimeSpan.FromSeconds(10);
                    opt.PermitLimit = 3;
                }).OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    context.HttpContext.Response.ContentType = "application/json";
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Result.Fail($"Çok fazla istekde bulundunuz. Lütfen sonra tekrar deneyin {retryAfter.TotalMinutes} dakika."))
                            , cancellationToken: token);
                    }
                    else
                    {
                        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Result.Fail($"Çok fazla istekde bulundunuz. Lütfen sonra tekrar deneyin."))
                            , cancellationToken: token);
                    }
                };
            });

            #endregion

            #region cors

            //CORSSettings corsSettings = configuration.GetSection("CORS")?.Get<CORSSettings>();
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowedOrigins", builder => builder
            //        .WithOrigins(corsSettings?.AllowedOrigins)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());
            //});

            #endregion

            #region cache

            var redisConnection = ConnectionMultiplexer.Connect(configuration["RedisConnection"]);
            services.AddSingleton<IConnectionMultiplexer>(redisConnection);

            #endregion
        }
    }
}
