<a id="readme-top"></a>

# Database

Scripts SQL para la gestión de la base de datos PostgreSQL del proyecto Task Management System.

---

## Archivos

| Archivo | Descripción |
|---------|-------------|
| `init.sql` | Script de inicialización con tablas, índices, triggers y datos de prueba |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Docker (Recomendado)

### Iniciar la base de datos

```bash
# Desde la raíz del proyecto
docker-compose up -d database

# Ver logs
docker-compose logs -f database

# Verificar estado
docker-compose ps
```

### Conectarse a la base de datos

```bash
# Usando docker exec
docker exec -it task_management_db psql -U postgres -d task_management_dev

# Usando cliente local
psql -h localhost -p 5432 -U postgres -d task_management_dev
```

### Reiniciar la base de datos

```bash
# Detener y eliminar volúmenes
docker-compose down -v

# Volver a iniciar
docker-compose up -d database
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Configuración Manual

### Crear la base de datos

```bash
psql -U postgres
```

```sql
CREATE DATABASE task_management_dev;
\q
```

### Ejecutar el script

```bash
psql -U postgres -d task_management_dev -f database/init.sql
```

### Verificar la instalación

```sql
\c task_management_dev
\dt
\d tasks
SELECT * FROM tasks WHERE is_deleted = FALSE;
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Credenciales

| Parámetro | Valor |
|-----------|-------|
| Host | `localhost` |
| Puerto | `5432` |
| Base de datos | `task_management_dev` |
| Usuario | `postgres` |
| Contraseña | `postgres123` |

### Connection String

```
Host=localhost;Port=5432;Database=task_management_dev;Username=postgres;Password=postgres123
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Estructura

### Tabla `tasks`

| Columna | Tipo | Descripción |
|---------|------|-------------|
| `id` | SERIAL | Clave primaria |
| `title` | VARCHAR(200) | Título (obligatorio) |
| `description` | TEXT | Descripción |
| `status` | VARCHAR(50) | Pending, InProgress, Completed, Cancelled |
| `priority` | VARCHAR(20) | Low, Medium, High |
| `due_date` | TIMESTAMP | Fecha de vencimiento |
| `created_at` | TIMESTAMP | Fecha de creación |
| `updated_at` | TIMESTAMP | Última actualización |
| `is_deleted` | BOOLEAN | Soft delete |

### Índices

| Índice | Columna | Descripción |
|--------|---------|-------------|
| `idx_tasks_status` | status | Filtro por estado |
| `idx_tasks_priority` | priority | Filtro por prioridad |
| `idx_tasks_due_date` | due_date | Ordenamiento por fecha |
| `idx_tasks_created_at` | created_at | Ordenamiento por creación |
| `idx_tasks_active` | status, priority | Consultas compuestas |

### Triggers

| Trigger | Descripción |
|---------|-------------|
| `trigger_tasks_updated_at` | Actualiza `updated_at` automáticamente en cada UPDATE |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

## Diagrama

```
┌─────────────────────────────────────────────────────────────┐
│                          tasks                              │
├─────────────────────────────────────────────────────────────┤
│ id           SERIAL        PRIMARY KEY                      │
│ title        VARCHAR(200)  NOT NULL                         │
│ description  TEXT                                           │
│ status       VARCHAR(50)   NOT NULL  DEFAULT 'Pending'      │
│ priority     VARCHAR(20)   NOT NULL  DEFAULT 'Medium'       │
│ due_date     TIMESTAMPTZ                                    │
│ created_at   TIMESTAMPTZ   NOT NULL  DEFAULT NOW()          │
│ updated_at   TIMESTAMPTZ   NOT NULL  DEFAULT NOW()          │
│ is_deleted   BOOLEAN       NOT NULL  DEFAULT FALSE          │
├─────────────────────────────────────────────────────────────┤
│ CONSTRAINTS                                                 │
│  - chk_tasks_status: status IN (Pending, InProgress, ...)   │
│  - chk_tasks_priority: priority IN (Low, Medium, High)      │
│  - chk_tasks_title_not_empty: LENGTH(TRIM(title)) > 0       │
└─────────────────────────────────────────────────────────────┘
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>
