using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Services;
using WebStore.Logging;
using WebStore.Services.Services;
using WebStore.Services.Services.Database;
using WebStore.WebApi.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddTransient<IDbInitializer, DbInitializer>();
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

builder.Services.AddScoped<IEmployeesData, EmployeeDataDb>();
builder.Services.AddScoped<IProductData, ProductDataDB>();
// builder.Services.AddScoped<ICartService,CartServiceCookies>();
builder.Services.AddScoped<IOrderService, OrderServiceDB>();

var app = builder.Build();
await using (var dbScope = app.Services.CreateAsyncScope())
{
    var dbInitializer = dbScope.ServiceProvider.GetRequiredService<IDbInitializer>();
    var token = new CancellationToken(false);
    await dbInitializer.InitializeAsync(false, token);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<MiddlewareExceptionHandling>();
app.MapControllers();

app.Run();