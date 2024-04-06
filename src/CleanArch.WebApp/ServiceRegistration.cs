using CleanArch.Application.Helpers;
using CleanArch.WebApp.Services;
using CleanArch.WebApp.Services.IdentityServer;
using CleanArch.WebApp.Services.Session;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Refit;


namespace CleanArch.WebApp
{
    public static class ServiceRegistration
    {
        public static void AddWebAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region services

            services.AddHttpContextAccessor(); //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddExceptionHandler<GlobalExceptionHandler>();

            #endregion

            #region session

            // Add distributed memory cache for session
            services.AddDistributedMemoryCache();

            // Configure session options
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // make the session cookie Essential
            });

            #endregion

            #region refit

            var baseApiUrl = configuration["BaseApiUrl"];
            var refitSettings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = (rq, ct) => SessionHelper.GetToken()
            };

            services.AddRefitClient<IAccountService>().ConfigureHttpClient(c => c.BaseAddress = new Uri(baseApiUrl + "/api/v1/account"));
            services.AddRefitClient<ICustomerService>(refitSettings).ConfigureHttpClient(c => c.BaseAddress = new Uri(baseApiUrl + "/api/v1/customer"));

            #endregion

            #region auth

            ////if you not use identity server

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //})
            //.AddCookie()
            //.AddGoogle(options =>  //Google
            //{
            //    options.ClientId = "XXXXXXXXXXXXXXXXXXX.apps.googleusercontent.com";
            //    options.ClientSecret = "XXXXXXXXXXXXXXXXXXXXX";
            //    options.SaveTokens = true;
            //});


            /////////////////////////////////////

            // identity server
            // in-memory, code config
            services.AddIdentityServer()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiScopes(Config.ApiScopes)
                    .AddInMemoryApiResources(Config.GetApis())
                    .AddInMemoryClients(Config.GetClients(configuration));

            services.AddAuthentication()
                    .AddOpenIdConnect(GoogleDefaults.AuthenticationScheme,
                       GoogleDefaults.DisplayName,
                       options =>
                       {
                           options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                           options.Authority = "https://accounts.google.com";
                           options.ClientId = "XXXXXXXXXXXXXXXXXXX.apps.googleusercontent.com";
                           options.ClientSecret = "XXXXXXXXXXXXXXXXXXXXX";
                           options.ResponseType = OpenIdConnectResponseType.IdToken;
                           options.CallbackPath = "/signin-google";
                           options.SaveTokens = true; //this has to be true to get the token value
                           options.Scope.Add("email");
                       });

            #endregion

            ServiceTool.CreateServiceProvider(services);
        }
    }
}
