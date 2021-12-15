var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var greetings = app.Configuration["CustomGreetings"];
app.MapGet("/", () => greetings);

app.Run();
