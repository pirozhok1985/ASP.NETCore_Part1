using WebStore.Infrastructure.Conventions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(param =>
{
    param.Conventions.Add(new TestConvention());
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.MapDefaultControllerRoute();

//app.MapGet("/", () => app.Configuration["CustomGreetings"]);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.UseStaticFiles();
app.Run();
