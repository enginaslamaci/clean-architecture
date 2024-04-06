using CleanArch.API;
using CleanArch.Application;
using CleanArch.Application.Enums;
using CleanArch.Application.Middlewares;
using CleanArch.Infrastructure;
using CleanArch.Infrastructure.Enums;
using CleanArch.Persistence;
using Serilog;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAPIServices(builder.Configuration);
builder.Services.AddStorage(StorageType.Local);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseSerilogRequestLogging();
app.UseSecurityHeaders();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCustomExceptionHandler(AppType.WebApi);
//app.UseCors("AllowedOrigins");
app.UseRateLimiter();
app.MapControllers();
app.Run();
