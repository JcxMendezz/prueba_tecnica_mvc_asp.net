using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Models.ViewModels;
using TaskManagementSystem.Web.Services.Interfaces;

namespace TaskManagementSystem.Web.Controllers.Api;

/// <summary>
/// API Controller para probar endpoints desde consola.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SuppressMessage("Style", "VSTHRD200:Use Async suffix", Justification = "API controller actions follow REST conventions")]
public class TasksApiController : ControllerBase
{
    private readonly ITaskService taskService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksApiController"/> class.
    /// </summary>
    /// <param name="taskService">The task service.</param>
    public TasksApiController(ITaskService taskService)
    {
        this.taskService = taskService;
    }

    /// <summary>
    /// Gets all tasks.
    /// </summary>
    /// <returns>List of all tasks.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await this.taskService.GetAllAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets a task by ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The task if found.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await this.taskService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="model">The task creation model.</param>
    /// <returns>The created task.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await this.taskService.CreateAsync(model);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="model">The task update model.</param>
    /// <returns>The updated task.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest(new { error = "ID mismatch" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await this.taskService.UpdateAsync(id, model);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes a task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>Success message.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await this.taskService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(new { message = result.Message });
    }
}
