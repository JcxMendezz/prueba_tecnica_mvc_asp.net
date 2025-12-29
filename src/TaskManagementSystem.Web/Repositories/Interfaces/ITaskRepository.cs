using TaskManagementSystem.Web.Models.Entities;
using TaskManagementSystem.Web.Models.ViewModels;

namespace TaskManagementSystem.Web.Repositories.Interfaces;

/// <summary>
/// Interfaz del repositorio para operaciones de tareas.
/// Define el contrato para acceso a datos.
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Obtiene todas las tareas con filtros opcionales.
    /// </summary>
    /// <param name="filter">Filtros a aplicar.</param>
    /// <returns>Lista de tareas.</returns>
    Task<IEnumerable<TaskEntity>> GetAllAsync(TaskFilterViewModel? filter = null);

    /// <summary>
    /// Obtiene una tarea por su ID.
    /// </summary>
    /// <param name="id">ID de la tarea.</param>
    /// <returns>La tarea encontrada o null.</returns>
    Task<TaskEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Crea una nueva tarea.
    /// </summary>
    /// <param name="entity">Entidad a crear.</param>
    /// <returns>La tarea creada con su ID asignado.</returns>
    Task<TaskEntity> CreateAsync(TaskEntity entity);

    /// <summary>
    /// Actualiza una tarea existente.
    /// </summary>
    /// <param name="entity">Entidad con los datos actualizados.</param>
    /// <returns>True si se actualizó correctamente.</returns>
    Task<bool> UpdateAsync(TaskEntity entity);

    /// <summary>
    /// Elimina una tarea por su ID.
    /// </summary>
    /// <param name="id">ID de la tarea a eliminar.</param>
    /// <returns>True si se eliminó correctamente.</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verifica si existe una tarea con el ID especificado.
    /// </summary>
    /// <param name="id">ID a verificar.</param>
    /// <returns>True si existe.</returns>
    Task<bool> ExistsAsync(int id);

    /// <summary>
    /// Obtiene el conteo de tareas por estado.
    /// </summary>
    /// <returns>Diccionario con estado y cantidad.</returns>
    Task<Dictionary<string, int>> GetCountByStatusAsync();
}
