using WebStore.Infrastructure.Conventions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(param =>
{
    param.Conventions.Add(new TestConvention());
});

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
