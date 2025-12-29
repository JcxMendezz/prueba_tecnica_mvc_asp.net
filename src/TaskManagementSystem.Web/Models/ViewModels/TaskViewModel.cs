using System.Globalization;
using TaskManagementSystem.Web.Models.Enums;

namespace TaskManagementSystem.Web.Models.ViewModels;

/// <summary>
/// ViewModel para mostrar una tarea.
/// </summary>
public class TaskViewModel
{
    /// <summary>Gets or sets the task ID.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the task title.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Gets or sets the task description.</summary>
    public string? Description { get; set; }

    /// <summary>Gets or sets the task status.</summary>
    public TaskItemStatus Status { get; set; }

    /// <summary>Gets or sets the task priority.</summary>
    public TaskPriority Priority { get; set; }

    /// <summary>Gets or sets the due date.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>Gets or sets the creation date.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the last update date.</summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>Gets the formatted status text.</summary>
    public string StatusText => Status switch
    {
        TaskItemStatus.Pending => "Pendiente",
        TaskItemStatus.InProgress => "En Progreso",
        TaskItemStatus.Completed => "Completada",
        TaskItemStatus.Cancelled => "Cancelada",
        _ => "Desconocido",
    };

    /// <summary>Gets the formatted priority text.</summary>
    public string PriorityText => Priority switch
    {
        TaskPriority.Low => "Baja",
        TaskPriority.Medium => "Media",
        TaskPriority.High => "Alta",
        _ => "Desconocida",
    };

    /// <summary>Gets the CSS class for status badge.</summary>
    public string StatusBadgeClass => Status switch
    {
        TaskItemStatus.Pending => "bg-warning text-dark",
        TaskItemStatus.InProgress => "bg-primary",
        TaskItemStatus.Completed => "bg-success",
        TaskItemStatus.Cancelled => "bg-secondary",
        _ => "bg-light text-dark",
    };

    /// <summary>Gets the CSS class for priority badge.</summary>
    public string PriorityBadgeClass => Priority switch
    {
        TaskPriority.Low => "bg-info text-dark",
        TaskPriority.Medium => "bg-warning text-dark",
        TaskPriority.High => "bg-danger",
        _ => "bg-light text-dark",
    };

    /// <summary>Gets a value indicating whether the task is overdue.</summary>
    public bool IsOverdue => DueDate.HasValue
        && DueDate.Value.Date < DateTime.Today
        && Status != TaskItemStatus.Completed
        && Status != TaskItemStatus.Cancelled;

    /// <summary>Gets the formatted due date.</summary>
    public string DueDateFormatted => DueDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "Sin fecha";

    /// <summary>Gets the formatted creation date.</summary>
    public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
}
