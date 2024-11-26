using Shopee.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext
builder.Services.AddDbContext<ShopporContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));

// Add Authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập
        options.LogoutPath = "/Account/Logout"; // Đường dẫn đăng xuất
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian tồn tại của cookie
    });

// Add Session services
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Bảo mật Cookie
    options.Cookie.IsEssential = true; // Cookie cần thiết
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của Session
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Session Middleware
app.UseSession(); // Cấu hình sử dụng Session phải được thêm trước Authentication và Authorization

// Add Authentication Middleware
app.UseAuthentication(); // Phải được thêm trước UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
