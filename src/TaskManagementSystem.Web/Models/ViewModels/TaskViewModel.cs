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

    /// <summary>Gets the CSS class for status badge (elegant style).</summary>
    public string StatusBadgeClass => Status switch
    {
        TaskItemStatus.Pending => "badge-pending",
        TaskItemStatus.InProgress => "badge-in-progress",
        TaskItemStatus.Completed => "badge-completed",
        TaskItemStatus.Cancelled => "badge-cancelled",
        _ => "badge-pending",
    };

    /// <summary>Gets the CSS class for priority badge (elegant style).</summary>
    public string PriorityBadgeClass => Priority switch
    {
        TaskPriority.Low => "badge-low",
        TaskPriority.Medium => "badge-medium",
        TaskPriority.High => "badge-high",
        _ => "badge-medium",
    };

    /// <summary>Gets the icon for status.</summary>
    public string StatusIcon => Status switch
    {
        TaskItemStatus.Pending => "bi-clock",
        TaskItemStatus.InProgress => "bi-arrow-repeat",
        TaskItemStatus.Completed => "bi-check-circle",
        TaskItemStatus.Cancelled => "bi-x-circle",
        _ => "bi-question-circle",
    };

    /// <summary>Gets the icon for priority.</summary>
    public string PriorityIcon => Priority switch
    {
        TaskPriority.Low => "bi-arrow-down",
        TaskPriority.Medium => "bi-dash",
        TaskPriority.High => "bi-arrow-up",
        _ => "bi-dash",
    };

    /// <summary>Gets a value indicating whether the task is overdue.</summary>
    public bool IsOverdue => DueDate.HasValue
        && DueDate.Value.Date < DateTime.Today
        && Status != TaskItemStatus.Completed
        && Status != TaskItemStatus.Cancelled;

    /// <summary>Gets a value indicating whether the task is completed.</summary>
    public bool IsCompleted => Status == TaskItemStatus.Completed;

    /// <summary>Gets the task card CSS class.</summary>
    public string CardClass
    {
        get
        {
            var classes = new List<string> { "task-card" };

            classes.Add(Priority switch
            {
                TaskPriority.Low => "priority-low",
                TaskPriority.Medium => "priority-medium",
                TaskPriority.High => "priority-high",
                _ => "priority-medium",
            });

            if (IsCompleted)
            {
                classes.Add("is-completed");
            }

            if (IsOverdue)
            {
                classes.Add("is-overdue");
            }

            return string.Join(" ", classes);
        }
    }

    /// <summary>Gets the table row CSS class.</summary>
    public string RowClass
    {
        get
        {
            if (IsOverdue)
            {
                return "is-overdue";
            }

            if (IsCompleted)
            {
                return "is-completed";
            }

            return string.Empty;
        }
    }

    /// <summary>Gets the formatted due date.</summary>
    public string DueDateFormatted => DueDate?.ToString("dd MMM yyyy", new CultureInfo("es-ES")) ?? "Sin fecha";

    /// <summary>Gets the relative due date text.</summary>
    public string DueDateRelative
    {
        get
        {
            if (!DueDate.HasValue)
            {
                return "Sin fecha";
            }

            var today = DateTime.Today;
            var dueDate = DueDate.Value.Date;
            var diff = (dueDate - today).Days;

            return diff switch
            {
                < 0 => $"Vencida hace {Math.Abs(diff)} días",
                0 => "Vence hoy",
                1 => "Vence mañana",
                <= 7 => $"Vence en {diff} días",
                _ => DueDateFormatted,
            };
        }
    }

    /// <summary>Gets the formatted creation date.</summary>
    public string CreatedAtFormatted => CreatedAt.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

    /// <summary>Gets short description for list view.</summary>
    public string ShortDescription => string.IsNullOrEmpty(Description)
        ? "Sin descripción"
        : Description.Length > 100
            ? Description[..97] + "..."
            : Description;
}
