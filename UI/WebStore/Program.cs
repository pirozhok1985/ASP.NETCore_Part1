using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Json;
using WebStore.DAL.Context;
using WebStore.Domain.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestAPI;
using WebStore.Logging;
using WebStore.Services.Services;
using WebStore.Services.Services.Cookies;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Identity;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Values;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net();
builder.Host.UseSerilog((host, loggerConfig) => loggerConfig.ReadFrom.Configuration(host.Configuration)
    // .MinimumLevel.Debug()
    // .MinimumLevel.Override("Microsoft",LogEventLevel.Warning)
    // .Enrich.FromLogContext()
    // .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
    // .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log")
    .WriteTo.File(new JsonFormatter(",",true),$@".\Logs\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json"));
builder.Services.AddControllersWithViews(param =>
{
    param.Conventions.Add(new TestConvention());
});
// builder.Services.AddSingleton<IEmployeesData,EmployeeDataInMemory>();
// builder.Services.AddScoped<IEmployeesData, EmployeeDataDb>();
//builder.Services.AddSingleton<IProductData, ProductDataInMemory>();
// builder.Services.AddScoped<IProductData, ProductDataDB>();
builder.Services.AddScoped<ICartService,CartServiceCookies>();
// builder.Services.AddScoped<IOrderService, OrderServiceDB>();
// builder.Services.AddHttpClient<IValueService,ValuesClient>(client => client.BaseAddress = new Uri(builder.Configuration["WebAPI"]));
// builder.Services.AddHttpClient<IEmployeesData,EmployeesClient>(client => client.BaseAddress = new Uri(builder.Configuration["WebAPI"]));
// builder.Services.AddHttpClient<IProductData,ProductsClient>(client => client.BaseAddress = new Uri(builder.Configuration["WebAPI"]));
// builder.Services.AddHttpClient<IOrderService,OrdersClient>(client => client.BaseAddress = new Uri(builder.Configuration["WebAPI"]));
builder.Services.AddHttpClient("WebStoreClient", client => client.BaseAddress = new Uri(builder.Configuration["WebAPI"]))
    .AddTypedClient<IValueService, ValuesClient>()
    .AddTypedClient<IEmployeesData, EmployeesClient>()
    .AddTypedClient<IProductData, ProductsClient>()
    .AddTypedClient<IOrderService, OrdersClient>();

builder.Services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddIdentity<User, Role>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;
#endif
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    opt.User.RequireUniqueEmail = false;

    opt.Lockout.AllowedForNewUsers = false;
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

}).AddDefaultTokenProviders();
builder.Services.AddHttpClient("WebIdentityClient",
        client => client.BaseAddress = new Uri(builder.Configuration["WebAPI"]))
    .AddTypedClient<IUserStore<User>, UsersClient>()
    .AddTypedClient<IUserLockoutStore<User>, UsersClient>()
    .AddTypedClient<IUserLoginStore<User>, UsersClient>()
    .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
    .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
    .AddTypedClient<IUserEmailStore<User>, UsersClient>()
    .AddTypedClient<IUserClaimStore<User>, UsersClient>()
    .AddTypedClient<IUserRoleStore<User>, UsersClient>()
    .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
    .AddTypedClient<IRoleStore<Role>, RolesClient>();
builder.Services.AddTransient<IDbInitializer, DbInitializer>();
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "WebStore.GB";
    //opt.Cookie.Expiration = TimeSpan.FromDays(10);
    opt.ExpireTimeSpan = TimeSpan.FromDays(10);
    opt.Cookie.HttpOnly = true;
    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";
    opt.SlidingExpiration = true;
});

var app = builder.Build();
await using (var dbScope = app.Services.CreateAsyncScope())
{
    var dbInitializer = dbScope.ServiceProvider.GetRequiredService<IDbInitializer>();
    var token = new CancellationToken(false);
    await dbInitializer.InitializeAsync(false, token);
}


#region Processing PipeLine

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<MiddlewareExceptionHandling>();
app.UseWelcomePage("/mswelcome");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();

#endregion
