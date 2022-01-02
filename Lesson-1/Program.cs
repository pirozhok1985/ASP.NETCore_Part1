using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(param =>
{
    param.Conventions.Add(new TestConvention());
});
builder.Services.AddSingleton<IEmployeesData,EmployeeDataInMemory>();
builder.Services.AddSingleton<IProductData, ProductDataInMemory>();
builder.Services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

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

app.UseWelcomePage("/mswelcome");
app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.Run();

#endregion
