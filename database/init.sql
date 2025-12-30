-- ===========================================
-- Script de inicialización de base de datos
-- Task Management System
-- PostgreSQL 16
-- ===========================================

-- ===========================================
-- Eliminar objetos existentes (para desarrollo)
-- ===========================================
DROP TRIGGER IF EXISTS trigger_tasks_updated_at ON tasks;
DROP FUNCTION IF EXISTS update_updated_at_column();
DROP TABLE IF EXISTS tasks CASCADE;

-- ===========================================
-- Tabla: Tasks (Tareas)
-- ===========================================
CREATE TABLE tasks (
    -- Clave primaria
    id SERIAL PRIMARY KEY,

    -- Campos principales
    title VARCHAR(200) NOT NULL,
    description TEXT,
    status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    priority VARCHAR(20) NOT NULL DEFAULT 'Medium',

    -- Fechas
    due_date TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,

    -- Soft delete (eliminación lógica)
    is_deleted BOOLEAN NOT NULL DEFAULT FALSE,

    -- Constraints de validación
    CONSTRAINT chk_tasks_status CHECK (status IN ('Pending', 'InProgress', 'Completed', 'Cancelled')),
    CONSTRAINT chk_tasks_priority CHECK (priority IN ('Low', 'Medium', 'High')),
    CONSTRAINT chk_tasks_title_not_empty CHECK (LENGTH(TRIM(title)) > 0)
);

-- ===========================================
-- Índices para optimización de consultas
-- ===========================================

-- Índice para filtrar por estado (consulta muy frecuente)
CREATE INDEX idx_tasks_status ON tasks(status) WHERE is_deleted = FALSE;

-- Índice para filtrar por prioridad
CREATE INDEX idx_tasks_priority ON tasks(priority) WHERE is_deleted = FALSE;

-- Índice para ordenar/filtrar por fecha de vencimiento
CREATE INDEX idx_tasks_due_date ON tasks(due_date) WHERE is_deleted = FALSE;

-- Índice para ordenar por fecha de creación (más recientes primero)
CREATE INDEX idx_tasks_created_at ON tasks(created_at DESC) WHERE is_deleted = FALSE;

-- Índice para búsqueda por texto (título y descripción)
CREATE INDEX idx_tasks_title_search ON tasks USING gin(to_tsvector('spanish', title)) WHERE is_deleted = FALSE;

-- Índice compuesto para consultas frecuentes (tareas activas)
CREATE INDEX idx_tasks_active ON tasks(status, priority, created_at DESC) WHERE is_deleted = FALSE;

-- ===========================================
-- Función para actualizar updated_at automáticamente
-- ===========================================
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger para actualizar updated_at en cada UPDATE
CREATE TRIGGER trigger_tasks_updated_at
    BEFORE UPDATE ON tasks
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- ===========================================
-- Datos de prueba
-- ===========================================
INSERT INTO tasks (title, description, status, priority, due_date) VALUES
    (
        'Configurar proyecto de Ingeniero Fredy Cuellar',
        'Configurar el proyecto inicial con todas las dependencias necesarias para el desarrollo.',
        'Completed',
        'High',
        CURRENT_TIMESTAMP + INTERVAL '1 day'
    ),
    (
        'Implementar CRUD de tareas Ing Mateo',
        'Crear las operaciones básicas para gestionar tareas: crear, leer, actualizar y eliminar.',
        'InProgress',
        'High',
        CURRENT_TIMESTAMP + INTERVAL '3 days'
    ),
    (
        'Diseñar interfaz de usuario',
        'Crear el diseño de la interfaz con Bootstrap 5 para una experiencia de usuario moderna.',
        'Pending',
        'Medium',
        CURRENT_TIMESTAMP + INTERVAL '5 days'
    ),
    (
        'Agregar validaciones',
        'Implementar validaciones tanto en frontend (JavaScript) como en backend (DataAnnotations).',
        'Pending',
        'Medium',
        CURRENT_TIMESTAMP + INTERVAL '7 days'
    ),
    (
        'Escribir documentación',
        'Documentar el proyecto, APIs y guías de uso para facilitar el mantenimiento.',
        'Pending',
        'Low',
        CURRENT_TIMESTAMP + INTERVAL '10 days'
    ),
    (
        'Tarea vencida de prueba',
        'Esta tarea ya venció para probar el estilo visual de tareas vencidas.',
        'Pending',
        'High',
        CURRENT_TIMESTAMP - INTERVAL '2 days'
    ),
    (
        'Revisar código',
        'Realizar code review del módulo de tareas antes del merge.',
        'InProgress',
        'Medium',
        CURRENT_TIMESTAMP + INTERVAL '1 day'
    ),
    (
        'Configurar CI/CD',
        'Configurar pipeline de integración continua con GitHub Actions.',
        'Pending',
        'Low',
        CURRENT_TIMESTAMP + INTERVAL '14 days'
    );

-- ===========================================
-- Verificar la instalación
-- ===========================================
DO $$
BEGIN
    RAISE NOTICE '==========================================';
    RAISE NOTICE 'Base de datos inicializada correctamente';
    RAISE NOTICE '==========================================';
END $$;

-- Mostrar resumen
SELECT
    'Tareas creadas' AS descripcion,
    COUNT(*) AS total
FROM tasks
WHERE is_deleted = FALSE;

SELECT
    status,
    COUNT(*) AS cantidad
FROM tasks
WHERE is_deleted = FALSE
GROUP BY status
ORDER BY status;
