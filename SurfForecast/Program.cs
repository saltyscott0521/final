using SurfForecast.DataAccess;
using Microsoft.EntityFrameworkCore;
using SurfForecast.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Setup EF connection
// https://stackoverflow.com/a/43098152/1385857
// https://medium.com/executeautomation/asp-net-core-6-0-minimal-api-with-entity-framework-core-69d0c13ba9ab
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration["Data:windy:ConnectionString"]));

var app = builder.Build();

//Initialize database schema with default Locations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();

    var location = new List<Location>
    {
    new Location{Name="Cocoa Beach, FL",Latitude=28.2125, Longitude=-80.5964},
    new Location{Name="St. Pete Beach, FL",Latitude=27.709, Longitude=-82.739},
    new Location{Name="North Shore, HI",Latitude=21.609, Longitude=158.096},
    };
    location.ForEach(s => dbContext.Locations.Add(s));
    dbContext.SaveChanges();
}




                

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
