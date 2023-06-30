using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

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
  });



var app = builder.Build();

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
