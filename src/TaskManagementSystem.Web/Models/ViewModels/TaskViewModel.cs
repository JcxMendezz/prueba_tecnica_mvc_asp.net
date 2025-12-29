using TaskManagementSystem.Web.Models.Enums;

namespace TaskManagementSystem.Web.Models.ViewModels;

/// <summary>
/// ViewModel para mostrar una tarea.
/// </summary>
public class TaskViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; }

    public TaskPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    /// <summary>Texto formateado del estado.</summary>
    public string StatusText => Status switch
    {
        TaskItemStatus.Pending => "Pendiente",
        TaskItemStatus.InProgress => "En Progreso",
        TaskItemStatus.Completed => "Completada",
        TaskItemStatus.Cancelled => "Cancelada",
        _ => "Desconocido",
    };

    /// <summary>Texto formateado de la prioridad.</summary>
    public string PriorityText => Priority switch
    {
        TaskPriority.Low => "Baja",
        TaskPriority.Medium => "Media",
        TaskPriority.High => "Alta",
        _ => "Desconocida",
    };

    /// <summary>Clase CSS para el badge de estado.</summary>
    public string StatusBadgeClass => Status switch
    {
        TaskItemStatus.Pending => "bg-warning text-dark",
        TaskItemStatus.InProgress => "bg-primary",
        TaskItemStatus.Completed => "bg-success",
        TaskItemStatus.Cancelled => "bg-secondary",
        _ => "bg-light text-dark",
    };

    /// <summary>Clase CSS para el badge de prioridad.</summary>
    public string PriorityBadgeClass => Priority switch
    {
        TaskPriority.Low => "bg-info text-dark",
        TaskPriority.Medium => "bg-warning text-dark",
        TaskPriority.High => "bg-danger",
        _ => "bg-light text-dark",
    };

    /// <summary>Indica si la tarea está vencida.</summary>
    public bool IsOverdue => DueDate.HasValue
        && DueDate.Value.Date < DateTime.Today
        && Status != TaskItemStatus.Completed
        && Status != TaskItemStatus.Cancelled;

    /// <summary>Fecha de vencimiento formateada.</summary>
    public string DueDateFormatted => DueDate?.ToString("dd/MM/yyyy") ?? "Sin fecha";

    /// <summary>Fecha de creación formateada.</summary>
    public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy HH:mm");
}
