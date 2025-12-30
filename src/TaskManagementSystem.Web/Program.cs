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

// Add services to the container con Runtime Compilation
var mvcBuilder = builder.Services.AddControllersWithViews();

// Habilitar recompilación de vistas en desarrollo (Hot Reload para .cshtml)
if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

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

// Health checks con verificación de base de datos
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "postgresql");

var app = builder.Build();

// ===========================================
// Middleware Pipeline
// ===========================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // En desarrollo, mostrar página de excepciones detallada
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Manejo de error 404 personalizado
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect("/Home/Error404");
    }
});

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
