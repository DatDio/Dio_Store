

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddTransient<IProductServicecs, ProductService>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
