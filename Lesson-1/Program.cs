using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Services;
using WebStore.Services.Database;
using WebStore.Services.Interfaces;
using WebStore.Services.Memory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(param =>
{
    param.Conventions.Add(new TestConvention());
});
// builder.Services.AddSingleton<IEmployeesData,EmployeeDataInMemory>();
builder.Services.AddScoped<IEmployeesData, EmployeeDataDB>();
//builder.Services.AddSingleton<IProductData, ProductDataInMemory>();
builder.Services.AddScoped<IProductData, ProductDataDB>();
builder.Services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer_NoteBook")));
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

}).AddEntityFrameworkStores<WebStoreDB>().AddDefaultTokenProviders();
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
await using (var db_scope = app.Services.CreateAsyncScope())
{
    var dbInitializer = db_scope.ServiceProvider.GetRequiredService<IDbInitializer>();
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
app.MapDefaultControllerRoute();

app.UseAuthentication();
app.UseAuthorization();

app.UseWelcomePage("/mswelcome");


app.Run();

#endregion
