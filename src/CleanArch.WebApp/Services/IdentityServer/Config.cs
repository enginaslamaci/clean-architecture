using IdentityServer4.Models;

namespace CleanArch.WebApp.Services.IdentityServer
{
    public static class Config
    {
        private static Client authorizationCodeFlowClient;

        public static IEnumerable<ApiScope> ApiScopes =>
            new[] { new ApiScope("api1.read_only"), new ApiScope("api1.full_access") };

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[] { new IdentityResources.OpenId(), new IdentityResources.Profile() };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "api1",
                    DisplayName = "Clean Architecture API",
                    Scopes = {"api1.full_access", "api1.read_only"}
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            authorizationCodeFlowClient = new Client
            {
                ClientId = "clenanarch",
                ClientName = "Clean Architecture",
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = { "openid", "profile", "api1.read_only", "api1.full_access" },
                AlwaysIncludeUserClaimsInIdToken = true,
                RequireConsent = false
            };

            return new[] { authorizationCodeFlowClient };
        }
    }
}
