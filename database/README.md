# Scripts de Base de Datos

Scripts SQL para la gestión de la base de datos PostgreSQL.

## 📁 Archivos

| Archivo | Descripción |
|---------|-------------|
| `init-database.sql` | Script de inicialización con tablas, índices y datos de prueba |

## 🐳 Usar con Docker (Recomendado)

### Iniciar la base de datos

```bash
# Desde la raíz del proyecto
docker-compose up -d database

# Ver logs
docker-compose logs -f database

# Verificar que está corriendo
docker-compose ps
```

### Conectarse a la base de datos

```bash
# Usando docker exec
docker exec -it task_management_db psql -U postgres -d task_management_dev

# O usando psql local
psql -h localhost -p 5432 -U postgres -d task_management_dev
# Password: postgres123
```

### Reiniciar la base de datos (borrar datos)

```bash
# Detener y eliminar volúmenes
docker-compose down -v

# Volver a iniciar (ejecutará init-database.sql automáticamente)
docker-compose up -d database
```

## 🔧 Configuración Manual (Sin Docker)

### 1. Crear la base de datos

```bash
# Conectarse a PostgreSQL
psql -U postgres

# Crear la base de datos
CREATE DATABASE task_management_dev;

# Salir
\q
```

### 2. Ejecutar el script

```bash
psql -U postgres -d task_management_dev -f scripts/init-database.sql
```

### 3. Verificar la instalación

```sql
-- Conectarse
\c task_management_dev

-- Listar tablas
\dt

-- Ver estructura
\d tasks

-- Ver datos
SELECT * FROM tasks WHERE is_deleted = FALSE;
```

## 🔑 Credenciales

| Campo | Valor |
|-------|-------|
| Host | `localhost` |
| Puerto | `5432` |
| Base de datos | `task_management_dev` |
| Usuario | `postgres` |
| Contraseña | `postgres123` |

## 📊 Connection String

```
Host=localhost;Port=5432;Database=task_management_dev;Username=postgres;Password=postgres123
```

## 🗃️ Estructura de la Tabla `tasks`

| Columna | Tipo | Descripción |
|---------|------|-------------|
| `id` | SERIAL | Clave primaria auto-incremental |
| `title` | VARCHAR(200) | Título de la tarea (obligatorio) |
| `description` | TEXT | Descripción detallada (opcional) |
| `status` | VARCHAR(50) | Estado: Pending, InProgress, Completed, Cancelled |
| `priority` | VARCHAR(20) | Prioridad: Low, Medium, High |
| `due_date` | TIMESTAMP | Fecha de vencimiento (opcional) |
| `created_at` | TIMESTAMP | Fecha de creación (automático) |
| `updated_at` | TIMESTAMP | Última actualización (automático) |
| `is_deleted` | BOOLEAN | Soft delete (default: FALSE) |
