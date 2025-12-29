using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Repositories;
using TaskManagementSystem.Web.Repositories.Interfaces;
using TaskManagementSystem.Web.Services;
using TaskManagementSystem.Web.Services.Interfaces;

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

// Configurar connection string para el factory
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Registrar DbConnectionFactory (Singleton porque es thread-safe)
builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new DbConnectionFactory(connectionString));

// Registrar Repositories (Scoped - una instancia por request)
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Registrar Services (Scoped - una instancia por request)
builder.Services.AddScoped<ITaskService, TaskService>();

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
