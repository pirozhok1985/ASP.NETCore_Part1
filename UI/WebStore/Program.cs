using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Formatting.Json;
using WebStore.DAL.Context;
using WebStore.Domain.Identity;
using WebStore.Hubs;
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
.WriteTo.File(new JsonFormatter(",",true),$@"/Logs/WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json"));
builder.Services.AddControllersWithViews(param =>
{
    param.Conventions.Add(new TestConvention());
});

builder.Services
    .AddHttpClient("WebStoreClient", client => client.BaseAddress = new Uri(builder.Configuration["WebAPI"]))
    .AddTypedClient<IValueService, ValuesClient>()
    .AddTypedClient<IEmployeesData, EmployeesClient>()
    .AddTypedClient<IProductData, ProductsClient>()
    .AddTypedClient<IOrderService, OrdersClient>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCercuitBreakerPolicy());

builder.Services.AddScoped<ICartStore, CartStoreCookies>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddIdentity<User, Role>()
    .AddDefaultTokenProviders();

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
    .AddTypedClient<IRoleStore<Role>, RolesClient>()
    .AddPolicyHandler(GetRetryPolicy());

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
builder.Services.AddSignalR();
var app = builder.Build();

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
    endpoints.MapHub<ChatHub>("/chat");
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

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int maxAttemptCount = 5, int maxJitterTime = 1000)
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(maxAttemptCount, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            + TimeSpan.FromMilliseconds(new Random().Next(2,maxJitterTime)));
}

static IAsyncPolicy<HttpResponseMessage> GetCercuitBreakerPolicy() => 
    HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));