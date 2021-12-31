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

var app = builder.Build();


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
