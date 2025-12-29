# Scripts de Base de Datos

Esta carpeta contiene los scripts SQL para la gestión de la base de datos.

## Archivos

| Archivo | Descripción |
|---------|-------------|
| `init-database.sql` | Script inicial para crear tablas e índices |

## Cómo usar

### 1. Crear la base de datos

```bash
# Conectarse a PostgreSQL
psql -U postgres

# Crear la base de datos
CREATE DATABASE task_management_dev;

# Salir
\q
```

### 2. Ejecutar el script de inicialización

```bash
# Opción 1: Desde la línea de comandos
psql -U postgres -d task_management_dev -f scripts/init-database.sql

# Opción 2: Desde psql
\c task_management_dev
\i scripts/init-database.sql
```

### 3. Verificar la instalación

```sql
-- Conectarse a la base de datos
\c task_management_dev

-- Listar tablas
\dt

-- Ver estructura de la tabla tasks
\d tasks

-- Contar registros
SELECT COUNT(*) FROM tasks;
```

## Configuración de conexión

Actualiza el archivo `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=task_management_dev;Username=postgres;Password=TU_PASSWORD"
  }
}
```

O usa variables de entorno:

```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=task_management_dev;Username=postgres;Password=TU_PASSWORD"
```
