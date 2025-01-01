using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Product_Management_Assessment;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register FastEndpoints services
builder.Services.AddFastEndpoints();

// Add authorization services (optional, if needed)
builder.Services.AddAuthorization();

var app = builder.Build();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/products");
        return;
    }

    await next();
});
// Use FastEndpoints middleware
app.UseFastEndpoints();

// Enable HTTPS redirection and static files
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure routing and authorization
app.UseRouting();
app.UseAuthorization();

app.Run();
