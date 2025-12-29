-- ===========================================
-- Script de inicialización de base de datos
-- Task Management System
-- ===========================================

-- Crear la base de datos (ejecutar como superusuario)
-- CREATE DATABASE task_management_dev;

-- Conectarse a la base de datos antes de ejecutar el resto
-- \c task_management_dev

-- ===========================================
-- Tabla: Tasks (Tareas)
-- ===========================================
CREATE TABLE IF NOT EXISTS tasks (
    id SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    priority VARCHAR(20) NOT NULL DEFAULT 'Medium',
    due_date TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,

    -- Constraints
    CONSTRAINT chk_status CHECK (status IN ('Pending', 'InProgress', 'Completed', 'Cancelled')),
    CONSTRAINT chk_priority CHECK (priority IN ('Low', 'Medium', 'High', 'Critical'))
);

-- ===========================================
-- Índices
-- ===========================================
CREATE INDEX IF NOT EXISTS idx_tasks_status ON tasks(status);
CREATE INDEX IF NOT EXISTS idx_tasks_priority ON tasks(priority);
CREATE INDEX IF NOT EXISTS idx_tasks_due_date ON tasks(due_date);
CREATE INDEX IF NOT EXISTS idx_tasks_created_at ON tasks(created_at);

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

-- Trigger para tasks
DROP TRIGGER IF EXISTS trigger_tasks_updated_at ON tasks;
CREATE TRIGGER trigger_tasks_updated_at
    BEFORE UPDATE ON tasks
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- ===========================================
-- Datos de prueba (opcional)
-- ===========================================
INSERT INTO tasks (title, description, status, priority, due_date)
VALUES
    ('Configurar proyecto', 'Configurar el proyecto inicial con todas las dependencias', 'Completed', 'High', CURRENT_TIMESTAMP + INTERVAL '1 day'),
    ('Implementar CRUD de tareas', 'Crear las operaciones básicas para gestionar tareas', 'InProgress', 'High', CURRENT_TIMESTAMP + INTERVAL '3 days'),
    ('Diseñar interfaz de usuario', 'Crear el diseño de la interfaz con Bootstrap', 'Pending', 'Medium', CURRENT_TIMESTAMP + INTERVAL '5 days'),
    ('Agregar validaciones', 'Implementar validaciones en frontend y backend', 'Pending', 'Medium', CURRENT_TIMESTAMP + INTERVAL '7 days'),
    ('Escribir documentación', 'Documentar el proyecto y las APIs', 'Pending', 'Low', CURRENT_TIMESTAMP + INTERVAL '10 days')
ON CONFLICT DO NOTHING;

-- ===========================================
-- Verificar la instalación
-- ===========================================
SELECT 'Base de datos inicializada correctamente' AS mensaje;
SELECT COUNT(*) AS total_tareas FROM tasks;
