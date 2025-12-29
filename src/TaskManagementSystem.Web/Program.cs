using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// ===========================================
// Configuración
// ===========================================

// Agregar soporte para variables de entorno
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// ===========================================
// Servicios
// ===========================================

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar la conexión a PostgreSQL
builder.Services.AddScoped<NpgsqlConnection>(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new NpgsqlConnection(connectionString);
});

// Health checks (opcional pero recomendado)
builder.Services.AddHealthChecks();

var app = builder.Build();

// ===========================================
// Middleware Pipeline
// ===========================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Health check endpoint
app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
