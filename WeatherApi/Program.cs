var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer("Bearer", options => {
    options.Audience = "weatherapi";
    options.Authority = "https://localhost:5001";

    // Ignore self-signed SSL
    // options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = null };
  });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => {
  policy
    // .AllowAnyOrigin()
    .WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
