using TaskManagementSystem.Web.Helpers.Results;
using TaskManagementSystem.Web.Models.ViewModels;

namespace TaskManagementSystem.Web.Services.Interfaces;

/// <summary>
/// Interfaz del servicio de tareas.
/// Define el contrato para la lógica de negocio.
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Obtiene todas las tareas con filtros opcionales.
    /// </summary>
    /// <param name="filter">Filtros a aplicar.</param>
    /// <returns>Resultado con la lista de tareas.</returns>
    Task<Result<TaskListViewModel>> GetAllAsync(TaskFilterViewModel? filter = null);

    /// <summary>
    /// Obtiene una tarea por su ID.
    /// </summary>
    /// <param name="id">ID de la tarea.</param>
    /// <returns>Resultado con la tarea encontrada.</returns>
    Task<Result<TaskViewModel>> GetByIdAsync(int id);

    /// <summary>
    /// Crea una nueva tarea.
    /// </summary>
    /// <param name="model">Datos de la tarea a crear.</param>
    /// <returns>Resultado con la tarea creada.</returns>
    Task<Result<TaskViewModel>> CreateAsync(TaskCreateViewModel model);

    /// <summary>
    /// Actualiza una tarea existente.
    /// </summary>
    /// <param name="id">ID de la tarea a actualizar.</param>
    /// <param name="model">Datos actualizados.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Result<TaskViewModel>> UpdateAsync(int id, TaskEditViewModel model);

    /// <summary>
    /// Elimina una tarea por su ID.
    /// </summary>
    /// <param name="id">ID de la tarea a eliminar.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Result> DeleteAsync(int id);

    /// <summary>
    /// Obtiene una tarea para edición.
    /// </summary>
    /// <param name="id">ID de la tarea.</param>
    /// <returns>Resultado con el ViewModel de edición.</returns>
    Task<Result<TaskEditViewModel>> GetForEditAsync(int id);
}
