﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyEStore.Entities;
using MyEStore.Models;
using MyEStore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyeStoreContext>(options => {
	options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(30);
});



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
		options.SlidingExpiration = true;
		options.LoginPath = "/Customer/Login";
		options.AccessDeniedPath = "/Forbidden/";
	});


//dang ky payment singleton
builder.Services.AddSingleton(x =>
	new PaypalClient(
		builder.Configuration["PayPalOptions:ClientId"],
		builder.Configuration["PayPalOptions:ClientSecret"],
		builder.Configuration["PayPalOptions:Mode"]
	)
);

//Connect VNPay API
builder.Services.AddSingleton<IVnPayService, VnPayService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();