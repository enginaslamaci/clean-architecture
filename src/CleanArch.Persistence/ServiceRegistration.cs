using CleanArch.Application.Abstractions.Persistence.Repositories;
using CleanArch.Application.Abstractions.Persistence.Services;
using CleanArch.Application.Abstractions.Persistence.Services.Authentications;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Persistence.Contexts;
using CleanArch.Persistence.Repositories;
using CleanArch.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Database

            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(
                   configuration.GetConnectionString("ConnStr"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    }
                 ));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            #endregion

            #region Repositories

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            #endregion

            #region Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();

            #endregion

        }
    }
}
