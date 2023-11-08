using Ads.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Configuration;
using Ads.Services.Services.Abstract;
using Ads.Services.Services.Concrete;
using Ads.Data.Services.Abstract;
using Ads.Data.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAdvertImageService, AdvertImageService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(DataRepository<>));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<TokenUsageService>();
builder.Services.AddScoped<ISearchService, SearchService>();
//builder.Services.AddAuthorization(options => ToDo:yetkilendirmelerde açılacak.
//{
//    options.AddPolicy("RequireAdministratorRole",
//        policy => policy.RequireRole("Admin"));
//});

// Uygulama inşa edilirken hizmetlerin oluşturulmasını ve veritabanının oluşturulup doldurulmasını sağlayın.
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    //context.Database.EnsureDeleted();
    bool isDatabaseCreated = context.Database.EnsureCreated();
    if (isDatabaseCreated)
    {
        DbSeeder.Seed(context);
    }
}

// Ortamı yapılandırın ve HTTP isteğini işlemeye başlayın.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "adminDelete",
//        pattern: "/Admin/AdvertComment/Delete/{id?}",
//        defaults: new { controller = "AdvertComment", action = "Delete" }
//    );
//});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "deleteComment",
    pattern: "Admin/AdvertComment/Delete/{id}",
    defaults: new { controller = "AdvertComment", action = "Delete" }
);


app.Run();
