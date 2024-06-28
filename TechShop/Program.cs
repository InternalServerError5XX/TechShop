using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TechShop;
using TechShop.Infrastructure;
using TechShop.Infrastructure.SeedData;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();
builder.Services.InitializeIdentity();
builder.InitializeAuthentication();
builder.Services.InitializeSwagger();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tech Shop API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "swagger",
    pattern: "swagger/{action=Index}/{id?}",
    defaults: new { controller = "Swagger", action = "Index" });

app.MapControllerRoute(
        name: "Error",
        pattern: "{*url}",
        defaults: new { controller = "Error", action = "Error404" }
    );

await app.CreateDefaultRoles();
await app.CreateDefaultUsers();

await app.RunAsync();
