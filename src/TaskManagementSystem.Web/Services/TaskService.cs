using TaskManagementSystem.Web.Helpers.Constants;
using TaskManagementSystem.Web.Helpers.Results;
using TaskManagementSystem.Web.Models.Entities;
using TaskManagementSystem.Web.Models.Enums;
using TaskManagementSystem.Web.Models.ViewModels;
using TaskManagementSystem.Web.Repositories.Interfaces;
using TaskManagementSystem.Web.Services.Interfaces;

namespace TaskManagementSystem.Web.Services;

/// <summary>
/// Implementación del servicio de tareas.
/// Contiene la lógica de negocio y validaciones.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository taskRepository;
    private readonly ILogger<TaskService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskService"/> class.
    /// </summary>
    /// <param name="taskRepository">The task repository.</param>
    /// <param name="logger">The logger instance.</param>
    public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
    {
        this.taskRepository = taskRepository;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Result<TaskListViewModel>> GetAllAsync(TaskFilterViewModel? filter = null)
    {
        try
        {
            this.logger.LogInformation("Getting all tasks with filter: {@Filter}", filter);

            var entities = await this.taskRepository.GetAllAsync(filter);
            var tasks = entities.Select(MapToViewModel).ToList();
            var counts = await this.taskRepository.GetCountByStatusAsync();

            var viewModel = new TaskListViewModel
            {
                Tasks = tasks,
                Filter = filter ?? new TaskFilterViewModel(),
                TotalCount = tasks.Count,
                PendingCount = counts.GetValueOrDefault("Pending"),
                InProgressCount = counts.GetValueOrDefault("InProgress"),
                CompletedCount = counts.GetValueOrDefault("Completed"),
                OverdueCount = counts.GetValueOrDefault("Overdue"),
            };

            return Result<TaskListViewModel>.Success(viewModel);
        }
        catch (Exception ex)
        {
            // Log detallado del error
            this.logger.LogError(ex, "Error getting all tasks. Exception: {Message}, StackTrace: {StackTrace}", ex.Message, ex.StackTrace);
            return Result<TaskListViewModel>.Failure($"Error: {ex.Message}");
        }
    }

    /// <inheritdoc/>
    public async Task<Result<TaskViewModel>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                return Result<TaskViewModel>.Failure(ErrorMessages.InvalidTaskId);
            }

            var entity = await this.taskRepository.GetByIdAsync(id);

            if (entity is null)
            {
                this.logger.LogWarning("Task not found: {TaskId}", id);
                return Result<TaskViewModel>.Failure(ErrorMessages.TaskNotFound);
            }

            return Result<TaskViewModel>.Success(MapToViewModel(entity));
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error getting task by ID: {TaskId}", id);
            return Result<TaskViewModel>.Failure(ErrorMessages.UnexpectedError);
        }
    }

    /// <inheritdoc/>
    public async Task<Result<TaskViewModel>> CreateAsync(TaskCreateViewModel model)
    {
        try
        {
            // Validaciones de negocio
            var validationResult = ValidateCreateModel(model);
            if (!validationResult.IsSuccess)
            {
                return Result<TaskViewModel>.Failure(validationResult.ErrorMessage!);
            }

            var entity = new TaskEntity
            {
                Title = model.Title.Trim(),
                Description = model.Description?.Trim(),
                Status = model.Status.ToString(),
                Priority = model.Priority.ToString(),
                DueDate = model.DueDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var created = await this.taskRepository.CreateAsync(entity);
            this.logger.LogInformation("Task created successfully: {TaskId}", created.Id);

            return Result<TaskViewModel>.Success(MapToViewModel(created), SuccessMessages.TaskCreated);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error creating task");
            return Result<TaskViewModel>.Failure(ErrorMessages.CreateError);
        }
    }

    /// <inheritdoc/>
    public async Task<Result<TaskViewModel>> UpdateAsync(int id, TaskEditViewModel model)
    {
        try
        {
            if (id <= 0 || model.Id != id)
            {
                return Result<TaskViewModel>.Failure(ErrorMessages.InvalidTaskId);
            }

            // Validaciones de negocio
            var validationResult = ValidateEditModel(model);
            if (!validationResult.IsSuccess)
            {
                return Result<TaskViewModel>.Failure(validationResult.ErrorMessage!);
            }

            // Verificar que existe
            var existing = await this.taskRepository.GetByIdAsync(id);
            if (existing is null)
            {
                return Result<TaskViewModel>.Failure(ErrorMessages.TaskNotFound);
            }

            // Actualizar propiedades
            existing.Title = model.Title.Trim();
            existing.Description = model.Description?.Trim();
            existing.Status = model.Status.ToString();
            existing.Priority = model.Priority.ToString();
            existing.DueDate = model.DueDate;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await this.taskRepository.UpdateAsync(existing);
            if (!updated)
            {
                return Result<TaskViewModel>.Failure(ErrorMessages.UpdateError);
            }

            this.logger.LogInformation("Task updated successfully: {TaskId}", id);

            // Obtener la entidad actualizada
            var updatedEntity = await this.taskRepository.GetByIdAsync(id);
            return Result<TaskViewModel>.Success(MapToViewModel(updatedEntity!), SuccessMessages.TaskUpdated);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error updating task: {TaskId}", id);
            return Result<TaskViewModel>.Failure(ErrorMessages.UpdateError);
        }
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                return Result.Failure(ErrorMessages.InvalidTaskId);
            }

            // Verificar que existe
            var exists = await this.taskRepository.ExistsAsync(id);
            if (!exists)
            {
                return Result.Failure(ErrorMessages.TaskNotFound);
            }

            var deleted = await this.taskRepository.DeleteAsync(id);
            if (!deleted)
            {
                return Result.Failure(ErrorMessages.DeleteError);
            }

            this.logger.LogInformation("Task deleted successfully: {TaskId}", id);
            return Result.Success(SuccessMessages.TaskDeleted);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error deleting task: {TaskId}", id);
            return Result.Failure(ErrorMessages.DeleteError);
        }
    }

    /// <inheritdoc/>
    public async Task<Result<TaskEditViewModel>> GetForEditAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                return Result<TaskEditViewModel>.Failure(ErrorMessages.InvalidTaskId);
            }

            var entity = await this.taskRepository.GetByIdAsync(id);

            if (entity is null)
            {
                return Result<TaskEditViewModel>.Failure(ErrorMessages.TaskNotFound);
            }

            var editModel = new TaskEditViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Status = Enum.Parse<TaskItemStatus>(entity.Status),
                Priority = Enum.Parse<TaskPriority>(entity.Priority),
                DueDate = entity.DueDate,
            };

            return Result<TaskEditViewModel>.Success(editModel);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error getting task for edit: {TaskId}", id);
            return Result<TaskEditViewModel>.Failure(ErrorMessages.UnexpectedError);
        }
    }

    private static TaskViewModel MapToViewModel(TaskEntity entity)
    {
        return new TaskViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Status = Enum.Parse<TaskItemStatus>(entity.Status),
            Priority = Enum.Parse<TaskPriority>(entity.Priority),
            DueDate = entity.DueDate,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }

    private static Result ValidateCreateModel(TaskCreateViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Title))
        {
            return Result.Failure(ErrorMessages.TitleRequired);
        }

        if (model.Title.Length > 200)
        {
            return Result.Failure(ErrorMessages.TitleTooLong);
        }

        if (model.Description?.Length > 2000)
        {
            return Result.Failure(ErrorMessages.DescriptionTooLong);
        }

        return Result.Success();
    }

    private static Result ValidateEditModel(TaskEditViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Title))
        {
            return Result.Failure(ErrorMessages.TitleRequired);
        }

        if (model.Title.Length > 200)
        {
            return Result.Failure(ErrorMessages.TitleTooLong);
        }

        if (model.Description?.Length > 2000)
        {
            return Result.Failure(ErrorMessages.DescriptionTooLong);
        }

        return Result.Success();
    }
}
