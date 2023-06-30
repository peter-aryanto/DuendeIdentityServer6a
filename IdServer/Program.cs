using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
  .AddInMemoryApiScopes(
    new List<ApiScope>
    {
      new ApiScope("weatherapi"),
    }
  )
  .AddInMemoryApiResources(
    new List<ApiResource>
    {
      new ApiResource("weatherapi")
    }
  )
  .AddInMemoryClients(new List<Client>{});



var app = builder.Build();

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
