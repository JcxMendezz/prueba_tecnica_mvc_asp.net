# ===========================================
# Dockerfile - Task Management System
# Multi-stage build para Render/Railway
# ===========================================

# ---------- Base Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["src/TaskManagementSystem.Web/TaskManagementSystem.Web.csproj", "src/TaskManagementSystem.Web/"]
RUN dotnet restore "src/TaskManagementSystem.Web/TaskManagementSystem.Web.csproj"

# Copiar todo el código fuente
COPY . .

# Compilar
WORKDIR "/src/src/TaskManagementSystem.Web"
RUN dotnet build "TaskManagementSystem.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---------- Publish Stage ----------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskManagementSystem.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------- Final Stage ----------
FROM base AS final
WORKDIR /app

# Copiar aplicación publicada
COPY --from=publish /app/publish .

# Copiar script de inicialización de BD (opcional, para referencia)
COPY database/init.sql /app/database/

# Variables de entorno por defecto
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Punto de entrada
ENTRYPOINT ["dotnet", "TaskManagementSystem.Web.dll"]
