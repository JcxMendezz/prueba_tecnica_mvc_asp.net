using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.Models.ViewModels;
using TaskManagementSystem.Web.Services.Interfaces;

namespace TaskManagementSystem.Web.Controllers.Api;

/// <summary>
/// API Controller para probar endpoints desde consola.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TasksApiController : ControllerBase
{
    private readonly ITaskService taskService;

    public TasksApiController(ITaskService taskService)
    {
        this.taskService = taskService;
    }

    /// <summary>
    /// GET: api/tasksapi.
    /// </summary>
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
    /// GET: api/tasksapi/5
    /// </summary>
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
    /// POST: api/tasksapi
    /// </summary>
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
    /// PUT: api/tasksapi/5
    /// </summary>
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
    /// DELETE: api/tasksapi/5
    /// </summary>
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
