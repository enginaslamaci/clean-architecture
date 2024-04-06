using CleanArch.WebApp;
using CleanArch.Application.Middlewares;
using Serilog;
using CleanArch.Application.Enums;
using CleanArch.WebApp.Extensions;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebAppServices(builder.Configuration);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    app.UseCustomExceptionHandler(AppType.WebApp);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.UseCustomExceptionHandler(AppType.WebApp);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
