<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![.NET][dotnet-shield]][dotnet-url]
[![PostgreSQL][postgresql-shield]][postgresql-url]
[![Docker][docker-shield]][docker-url]
[![License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h1 align="center">Task Management System</h1>

  <p align="center">
    Sistema de gestión de tareas desarrollado con ASP.NET Core MVC y PostgreSQL
    <br />
    <a href="#getting-started"><strong>Comenzar »</strong></a>
    <br />
    <br />
    <a href="#usage">Ver Demo</a>
    ·
    <a href="https://github.com/JcxMendezz/prueba_tecnica_mvc_asp.net/issues">Reportar Bug</a>
    ·
    <a href="https://github.com/JcxMendezz/prueba_tecnica_mvc_asp.net/issues">Solicitar Feature</a>
  </p>
</div>

---

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Tabla de Contenidos</summary>
  <ol>
    <li>
      <a href="#about-the-project">Acerca del Proyecto</a>
      <ul>
        <li><a href="#built-with">Tecnologías</a></li>
        <li><a href="#architecture">Arquitectura</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Comenzar</a>
      <ul>
        <li><a href="#prerequisites">Prerrequisitos</a></li>
        <li><a href="#installation">Instalación</a></li>
      </ul>
    </li>
    <li><a href="#usage">Uso</a></li>
    <li><a href="#api-reference">API Reference</a></li>
    <li><a href="#database">Base de Datos</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#license">Licencia</a></li>
    <li><a href="#contact">Contacto</a></li>
  </ol>
</details>

---

<!-- ABOUT THE PROJECT -->
## About The Project

Sistema web para la gestión de tareas que permite crear, visualizar, editar y eliminar tareas con funcionalidades avanzadas de filtrado, búsqueda y ordenamiento.

Características principales:

- CRUD completo de tareas con validaciones
- Filtros por estado (Pendiente, En Progreso, Completada, Cancelada)
- Filtros por prioridad (Baja, Media, Alta)
- Búsqueda por título y descripción
- Soft delete (eliminación lógica)
- API REST para integración
- Contenedorización con Docker

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

- [![.NET][dotnet-shield]][dotnet-url]
- [![PostgreSQL][postgresql-shield]][postgresql-url]
- [![Docker][docker-shield]][docker-url]
- [![Bootstrap][bootstrap-shield]][bootstrap-url]
- [![jQuery][jquery-shield]][jquery-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Architecture

El proyecto implementa una arquitectura en capas con separación de responsabilidades:

```text
TaskManagementSystem.Web/
├── Controllers/          # Controladores MVC y API
├── Services/             # Lógica de negocio
├── Repositories/         # Acceso a datos (Dapper)
├── Models/
│   ├── Entities/         # Entidades de base de datos
│   ├── ViewModels/       # Modelos de vista
│   └── Enums/            # Enumeraciones
├── Data/                 # Factory de conexión
├── Helpers/              # Utilidades y constantes
└── Views/                # Vistas Razor
```

Patrones implementados:

- Repository Pattern
- Dependency Injection
- Result Pattern
- Factory Pattern

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- GETTING STARTED -->
## Getting Started

Instrucciones para configurar el proyecto localmente.

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (recomendado)
- [PostgreSQL 16](https://www.postgresql.org/download/) (si no usa Docker)

### Installation

1. Clonar el repositorio

  **HTTPS:**

  ```sh
  git clone https://github.com/JcxMendezz/prueba_tecnica_mvc_asp.net.git
  cd prueba_tecnica_mvc_asp.net
  ```

  **SSH:**

  ```sh
  git clone git@github.com:JcxMendezz/prueba_tecnica_mvc_asp.net.git
  cd prueba_tecnica_mvc_asp.net
  ```

1. Iniciar la base de datos con Docker

    ```sh
    docker-compose up -d database
    ```

2. Verificar que el contenedor está corriendo

    ```sh
    docker-compose ps
    ```

3. Configurar el connection string en `src/TaskManagementSystem.Web/appsettings.Development.json`

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5433;Database=task_management_dev;Username=postgres;Password=keymaster?"
      }
    }
    ```

4. Ejecutar la aplicación

    ```sh
    cd src/TaskManagementSystem.Web
    dotnet run
    ```

5. Acceder a la aplicación en `http://localhost:5236`

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- USAGE -->
## Usage

### Interfaz Web

Acceder a `http://localhost:5236` para usar la interfaz gráfica.

### API REST

Ejemplos de uso con PowerShell:

Obtener todas las tareas:

```powershell
Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi" -Method Get | ConvertTo-Json
```

Crear una tarea:

```powershell
Write-Host "`n[3] POST - Crear nueva tarea" -ForegroundColor Yellow
$newTask = @{
    title = "Nueva tarea de prueba"
    description = "Creada desde script de prueba"
    status = 0
    priority = 1
    dueDate = "2026-02-20"
} | ConvertTo-Json

$created = Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi" -Method Post -Body $newTask -ContentType "application/json"
$created | ConvertTo-Json -Depth 3
$newId = $created.id
Write-Host "Tarea creada con ID: $newId" -ForegroundColor Cyan

```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- API REFERENCE -->
## API Reference

### Endpoints

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/tasksapi` | Obtener todas las tareas |
| `GET` | `/api/tasksapi/{id}` | Obtener tarea por ID |
| `POST` | `/api/tasksapi` | Crear nueva tarea |
| `PUT` | `/api/tasksapi/{id}` | Actualizar tarea |
| `DELETE` | `/api/tasksapi/{id}` | Eliminar tarea (soft delete) |

### Enumeraciones

**Status:**

| Valor | Descripción |
|-------|-------------|
| `0` | Pending |
| `1` | InProgress |
| `2` | Completed |
| `3` | Cancelled |

**Priority:**

| Valor | Descripción |
|-------|-------------|
| `0` | Low |
| `1` | Medium |
| `2` | High |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- DATABASE -->
## Database

### Configuración con Docker

```sh
# Iniciar base de datos
docker-compose up -d database

# Ver logs
docker-compose logs -f database

# Reiniciar (elimina datos)
docker-compose down -v
docker-compose up -d database
```

### Credenciales por defecto

| Parámetro | Valor |
|-----------|-------|
| Host | `localhost` |
| Puerto | `5433` |
| Base de datos | `task_management_dev` |
| Usuario | `postgres` |
| Contraseña | `keymaster?` |

### Estructura de la tabla tasks

| Columna | Tipo | Descripción |
|---------|------|-------------|
| `id` | SERIAL | Clave primaria |
| `title` | VARCHAR(200) | Título (obligatorio) |
| `description` | TEXT | Descripción |
| `status` | VARCHAR(50) | Estado |
| `priority` | VARCHAR(20) | Prioridad |
| `due_date` | TIMESTAMP | Fecha de vencimiento |
| `created_at` | TIMESTAMP | Fecha de creación |
| `updated_at` | TIMESTAMP | Última actualización |
| `is_deleted` | BOOLEAN | Soft delete |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- ROADMAP -->
## Roadmap

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- TESTING & IMPROVEMENTS -->
## Testing & Mejoras Sugeridas

### Sugerencias de Pruebas Manuales

- Crear, editar y eliminar tareas desde la interfaz web y vía API
- Validar errores de formulario (campos obligatorios, fechas inválidas)
- Probar filtros y búsqueda en la lista de tareas
- Verificar paginación y ordenamiento
- Probar la experiencia en dispositivos móviles (responsive)
- Simular errores 404 y validar la página personalizada
- Probar validaciones de backend enviando datos inválidos vía API

### Ejemplos de Uso Avanzado de la API

Actualizar una tarea existente:

```powershell
$body = @{
  id = 1
  title = "Tarea actualizada"
  description = "Nueva descripción"
  status = 2
  priority = 2
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi/1" -Method Put -Body $body -ContentType "application/json"
```

Eliminar (soft delete) una tarea:

```powershell
Invoke-RestMethod -Uri "http://localhost:5236/api/tasksapi/1" -Method Delete
```

### Posibles Mejoras Futuras

- Autenticación y autorización de usuarios
- Gestión de usuarios y roles
- Historial de cambios y auditoría
- Notificaciones por email o push
- Exportar tareas a Excel/CSV
- Integración con servicios externos (Slack, Teams, etc.)
- Tests automatizados (unitarios y de integración)
- Mejoras de accesibilidad (a11y)

## License

Distribuido bajo la licencia MIT. Ver `LICENSE` para más información.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- CONTACT -->
## Contact

Juan Camilo Méndez - [@JcxMendezz](https://github.com/JcxMendezz)

Project Link: [https://github.com/JcxMendezz/prueba_tecnica_mvc_asp.net](https://github.com/JcxMendezz/prueba_tecnica_mvc_asp.net)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- MARKDOWN LINKS & IMAGES -->
[dotnet-shield]: https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[dotnet-url]: https://dotnet.microsoft.com/
[postgresql-shield]: https://img.shields.io/badge/PostgreSQL-16-4169E1?style=for-the-badge&logo=postgresql&logoColor=white
[postgresql-url]: https://www.postgresql.org/
[docker-shield]: https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white
[docker-url]: https://www.docker.com/
[bootstrap-shield]: https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white
[bootstrap-url]: https://getbootstrap.com/
[jquery-shield]: https://img.shields.io/badge/jQuery-3.7-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[jquery-url]: https://jquery.com/
[license-shield]: https://img.shields.io/github/license/JcxMendezz/prueba_tecnica_mvc_asp.net?style=for-the-badge
[license-url]: https://github.com/JcxMendezz/prueba_tecnica_mvc_asp.net/blob/main/LICENSE
[linkedin-shield]: https://img.shields.io/badge/LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white
[linkedin-url]: https://linkedin.com/in/
