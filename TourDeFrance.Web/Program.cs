using Microsoft.EntityFrameworkCore;
using TourDeFrance.Data;
using TourDeFrance.Web.Components;
using TourDeFrance.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

var databaseProvider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";

builder.Services.AddDbContext<TourDeFranceDbContext>(options =>
{
    if (databaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(connectionString,
            x => x.MigrationsAssembly("TourDeFrance.Data.SqlServer"));
    }
    else if (databaseProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlite(connectionString);
    }
    else
    {
        throw new InvalidOperationException(
            $"Unsupported DatabaseProvider '{databaseProvider}'. Valid values are 'Sqlite' and 'SqlServer'.");
    }
});

builder.Services.AddScoped<RennerService>();
builder.Services.AddScoped<DeelnemerService>();
builder.Services.AddScoped<EtappeService>();
builder.Services.AddScoped<PuntenService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Migrate database and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TourDeFranceDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
