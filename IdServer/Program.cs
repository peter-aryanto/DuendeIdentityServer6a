/*
  https://www.youtube.com/watch?v=Tv4jU1rLVOo (Federated Identity: An intro to OAuth2, Open Id Connect & Duende IdentityServer 5 | Anthony Nguyen)

  Quickstart: https://github.com/DuendeSoftware/IdentityServer.Quickstart.UI
  curl -L https://raw.githubusercontent.com/DuendeSoftware/IdentityServer.Quickstart.UI/main/getmain.sh | bash
*/
using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => {
  policy
    // .AllowAnyOrigin()
    .WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod();
}));

// builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddIdentityServer()
  .AddInMemoryApiScopes(
    new List<ApiScope>
    {
      new ApiScope("weatherapi.read", "Read access to weatherapi"),
    }
  )
  .AddInMemoryApiResources(
    new List<ApiResource>
    {
      new ApiResource("weatherapi")
      {
        Scopes = { "weatherapi.read" }
      },
    }
  )
  .AddInMemoryClients(new List<Client>{
    new Client
    {
      ClientId = "m2m",
      ClientSecrets = { new Secret("m2msecret".Sha256()) },
      AllowedGrantTypes = GrantTypes.ClientCredentials,
      AllowedScopes = { "weatherapi.read" },
    },
    new Client
    {
      ClientId = "weather-client",
      RequireClientSecret = false,
      AllowedGrantTypes = GrantTypes.Code,
      AllowedScopes = { "openid", "profile", "weatherapi.read" },

      RedirectUris = { "http://localhost:3000/signin-oidc" },
      PostLogoutRedirectUris = { "Http://localhost:3000" },

      RequireConsent = true,
    }
  })
  .AddInMemoryIdentityResources(new List<IdentityResource>{
    new IdentityResources.OpenId(),
    new IdentityResources.Profile(),
  })
  .AddTestUsers(IdentityServerHost.TestUsers.Users);



var app = builder.Build();

app.UseCors();

app.UseIdentityServer();

// app.MapGet("/", () => "Hello World!");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
// app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.MapRazorPages().RequireAuthorization();

app.Run();
