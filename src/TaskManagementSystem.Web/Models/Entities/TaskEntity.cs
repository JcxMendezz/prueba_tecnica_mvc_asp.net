namespace TaskManagementSystem.Web.Models.Entities;

/// <summary>
/// Entidad que representa una tarea en la base de datos.
/// </summary>
public class TaskEntity
{
    /// <summary>Gets or sets the unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the task title.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Gets or sets the detailed description.</summary>
    public string? Description { get; set; }

    /// <summary>Gets or sets the current status (as string for DB mapping).</summary>
    public string Status { get; set; } = "Pending";

    /// <summary>Gets or sets the priority level (as string for DB mapping).</summary>
    public string Priority { get; set; } = "Medium";

    /// <summary>Gets or sets the due date.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>Gets or sets the creation date.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the last update date.</summary>
    public DateTime UpdatedAt { get; set; }
}
