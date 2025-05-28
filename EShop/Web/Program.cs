using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Services.Interfaces;
using Services;
using Data;
using BundlerMinifier.TagHelpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddNewtonsoftJson();
builder.Services.AddControllers();
builder.Services.AddMvc();

builder.Services.TryAddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
{
    ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
    HostAppId = "Html5ReportViewerDemo",
    Storage = new FileStorage(),
    ReportSourceResolver = new UriReportSourceResolver(
        Path.Combine(GetReportsDir(sp)))
});

builder.Services.AddDbContext<EShopDatabaseContext>(options =>
{
    var wwwrootDirectory = builder.Environment.WebRootPath;
    var fileName = "demos.db";

    options.UseSqlite(@"Data Source=" + wwwrootDirectory + Path.DirectorySeparatorChar + @"demos.db;");
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.Cookie.Name = ".EShop.Authentication.Cookie";
    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".EShop.Session";
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddBundles(options =>
{
	options.AppendVersion = true;
});
builder.Services.AddKendo();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IOrderService, OrderService>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");    
    app.UseHsts(); // The default HSTS value is 30 days. More: https://aka.ms/aspnetcore-hsts.
}
app.UseHttpsRedirection();
app.UsePathBase("/aspnet-core/eshop/");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	// ... 
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static string GetReportsDir(IServiceProvider sp)
{
    var iwhe = sp.GetService<IWebHostEnvironment>();
    if (iwhe == null)
    {
        throw new ArgumentNullException(nameof(sp));
    }
	return Path.Combine(iwhe.ContentRootPath, "Reports");
}
