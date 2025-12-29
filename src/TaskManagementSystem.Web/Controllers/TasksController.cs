using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Models.Enums;
using TaskManagementSystem.Web.Models.ViewModels;
using TaskManagementSystem.Web.Services.Interfaces;

namespace TaskManagementSystem.Web.Controllers;

/// <summary>
/// Controlador para gestión de tareas.
/// Implementa CRUD completo con manejo de errores.
/// </summary>
public class TasksController : Controller
{
    private readonly ITaskService taskService;
    private readonly ILogger<TasksController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksController"/> class.
    /// </summary>
    /// <param name="taskService">The task service.</param>
    /// <param name="logger">The logger instance.</param>
    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        this.taskService = taskService;
        this.logger = logger;
    }

    /// <summary>
    /// GET: /Tasks - Shows the task list with optional filters.
    /// </summary>
    /// <param name="filter">Optional filter parameters.</param>
    /// <returns>The task list view.</returns>
    [HttpGet]
    public async Task<IActionResult> IndexAsync([FromQuery] TaskFilterViewModel? filter)
    {
        try
        {
            var result = await this.taskService.GetAllAsync(filter);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return View(new TaskListViewModel());
            }

            return View(result.Value);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error loading task list");
            TempData["ErrorMessage"] = "Error al cargar la lista de tareas.";
            return View(new TaskListViewModel());
        }
    }

    /// <summary>
    /// GET: /Tasks/Details/5 - Shows task details.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The task details view.</returns>
    [HttpGet]
    public async Task<IActionResult> DetailsAsync(int id)
    {
        try
        {
            var result = await this.taskService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction(nameof(IndexAsync));
            }

            return View(result.Value);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error loading task details: {TaskId}", id);
            TempData["ErrorMessage"] = "Error al cargar los detalles de la tarea.";
            return RedirectToAction(nameof(IndexAsync));
        }
    }

    /// <summary>
    /// GET: /Tasks/Create - Shows the create task form.
    /// </summary>
    /// <returns>The create task view.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View(new TaskCreateViewModel());
    }

    /// <summary>
    /// POST: /Tasks/Create - Creates a new task.
    /// </summary>
    /// <param name="model">The task creation model.</param>
    /// <returns>Redirect to details or the form with errors.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskCreateViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.taskService.CreateAsync(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(DetailsAsync), new { id = result.Value!.Id });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error creating task");
            ModelState.AddModelError(string.Empty, "Error al crear la tarea.");
            return View(model);
        }
    }

    /// <summary>
    /// GET: /Tasks/Edit/5 - Shows the edit task form.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The edit task view.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var result = await this.taskService.GetForEditAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction(nameof(IndexAsync));
            }

            return View(result.Value);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error loading task for edit: {TaskId}", id);
            TempData["ErrorMessage"] = "Error al cargar la tarea para editar.";
            return RedirectToAction(nameof(IndexAsync));
        }
    }

    /// <summary>
    /// POST: /Tasks/Edit/5 - Updates a task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="model">The task edit model.</param>
    /// <returns>Redirect to details or the form with errors.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskEditViewModel model)
    {
        try
        {
            if (id != model.Id)
            {
                TempData["ErrorMessage"] = "ID de tarea inválido.";
                return RedirectToAction(nameof(IndexAsync));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.taskService.UpdateAsync(id, model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(DetailsAsync), new { id });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error updating task: {TaskId}", id);
            ModelState.AddModelError(string.Empty, "Error al actualizar la tarea.");
            return View(model);
        }
    }

    /// <summary>
    /// GET: /Tasks/Delete/5 - Shows delete confirmation.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The delete confirmation view.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await this.taskService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction(nameof(IndexAsync));
            }

            return View(result.Value);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error loading task for delete: {TaskId}", id);
            TempData["ErrorMessage"] = "Error al cargar la tarea.";
            return RedirectToAction(nameof(IndexAsync));
        }
    }

    /// <summary>
    /// POST: /Tasks/Delete/5 - Deletes a task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>Redirect to index.</returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var result = await this.taskService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction(nameof(IndexAsync));
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(IndexAsync));
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error deleting task: {TaskId}", id);
            TempData["ErrorMessage"] = "Error al eliminar la tarea.";
            return RedirectToAction(nameof(IndexAsync));
        }
    }

    /// <summary>
    /// POST: /Tasks/UpdateStatus/5 - Quick status update via AJAX.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="request">The status update request.</param>
    /// <returns>JSON result.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateRequest request)
    {
        try
        {
            var taskResult = await this.taskService.GetForEditAsync(id);
            if (!taskResult.IsSuccess)
            {
                return Json(new { success = false, message = taskResult.ErrorMessage });
            }

            var editModel = taskResult.Value!;
            editModel.Status = request.Status;

            var result = await this.taskService.UpdateAsync(id, editModel);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.ErrorMessage });
            }

            return Json(new { success = true, message = result.Message });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error updating task status: {TaskId}", id);
            return Json(new { success = false, message = "Error al actualizar el estado." });
        }
    }
}

/// <summary>
/// Request model for AJAX status update.
/// </summary>
/// <param name="Status">The new status value.</param>
public record StatusUpdateRequest(TaskItemStatus Status);
