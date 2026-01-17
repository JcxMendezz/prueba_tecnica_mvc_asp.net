using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Repositories;
using TaskManagementSystem.Web.Repositories.Interfaces;
using TaskManagementSystem.Web.Services;
using TaskManagementSystem.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ===========================================
// Configuración de Puerto (Solo producción/Railway)
// ===========================================
if (!builder.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// ===========================================
// Configuración
// ===========================================
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// ===========================================
// Connection String (Render/Railway compatible)
// ===========================================
string? connectionString = null;

// 1. DATABASE_URL de Render (formato postgres://user:pass@host:port/db)
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    try
    {
        // Render puede enviar postgres:// o postgresql://
#pragma warning disable CA1310 // Specify StringComparison for correctness
        if (databaseUrl.StartsWith("postgres://"))
        {
            databaseUrl = string.Concat("postgresql://", databaseUrl.AsSpan(11));
        }
#pragma warning restore CA1310 // Specify StringComparison for correctness

        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        var host = uri.Host;
        var portDb = uri.Port > 0 ? uri.Port : 5432;
        var database = uri.AbsolutePath.TrimStart('/');
        var username = userInfo.Length > 0 ? userInfo[0] : "postgres";
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;

        connectionString = $"Host={host};Port={portDb};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

        Console.WriteLine($"[Config] Using DATABASE_URL: Host={host}, Database={database}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Config] Error parsing DATABASE_URL: {ex.Message}");
    }
}

// 2. Variables individuales de Render
if (string.IsNullOrEmpty(connectionString))
{
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
    var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

    if (!string.IsNullOrEmpty(dbHost) && !string.IsNullOrEmpty(dbName))
    {
        connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

        if (!builder.Environment.IsDevelopment())
        {
            connectionString += ";SSL Mode=Require;Trust Server Certificate=true";
        }

        Console.WriteLine($"[Config] Using DB_* variables: Host={dbHost}, Database={dbName}");
    }
}

// 3. Variable directa ConnectionStrings__DefaultConnection
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString))
    {
        Console.WriteLine("[Config] Using ConnectionStrings__DefaultConnection");
    }
}

// 4. Fallback a appsettings.json
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString))
    {
        Console.WriteLine("[Config] Using appsettings.json connection string");
    }
}

// Validar
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException(
        "Database connection string not configured. " +
        "Set DATABASE_URL, ConnectionStrings__DefaultConnection, or DB_* environment variables.");
}

// ===========================================
// Servicios
// ===========================================
var mvcBuilder = builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new DbConnectionFactory(connectionString));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "postgresql");

var app = builder.Build();

// ===========================================
// Middleware Pipeline
// ===========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// No usar HTTPS redirect en contenedores (Render maneja SSL)
if (!app.Environment.IsDevelopment())
{
    // Render termina SSL en el proxy
    app.UseForwardedHeaders();
}
else
{
    app.UseHttpsRedirection();
}

app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect("/Home/Error404");
    }

    return Task.CompletedTask;
});

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
