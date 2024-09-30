using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebApplication1ContextConnection") ?? throw new InvalidOperationException("Connection string 'WebApplication1ContextConnection' not found.");

builder.Services.AddDbContext<WebApplication1Context>(options => options.UseOracle(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebApplication1Context>();
// Добавляем поддержку сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Устанавливаем время ожидания
    options.Cookie.HttpOnly = true; // Защита от JavaScript
    options.Cookie.IsEssential = true; // Необходимая кука для работы сессий
});


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action}");

app.MapControllerRoute(
            name: "AuthController",
            pattern: "{action}",
            defaults: new { controller = "Auth", action = "{action}" });





app.MapControllerRoute(
            name: "AdminController",
            pattern: "{action}",
            defaults: new { controller = "Admin", action = "{action}" });




app.MapControllerRoute(
            name: "UserController",
            pattern: "{action}",
            defaults: new { controller = "User", action = "{action}" });


app.MapDefaultControllerRoute();

app.Run();
