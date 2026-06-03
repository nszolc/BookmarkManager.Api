using BookmarkManager.Api.Data;
using BookmarkManager.Api.Middleware;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<Program>(); //register FluentValidation validators

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();


app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

// Clean URLs for the frontend: the browser keeps the pretty path
// (e.g. /favorites, /categories) while we serve the matching static
// HTML file internally. This keeps copy-link and the Back button working.
var pageRoutes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    ["/favorites"] = "/index.html",
    ["/categories"] = "/categories.html",
    ["/tags"] = "/tags.html",
};
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;
    if (path is not null && pageRoutes.TryGetValue(path, out var file))
    {
        context.Request.Path = file;
    }
    await next();
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
