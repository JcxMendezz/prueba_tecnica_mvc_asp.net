namespace TaskManagementSystem.Web.Models.Enums;

/// <summary>
/// Estados posibles de una tarea.
/// </summary>
public enum TaskItemStatus
{
    /// <summary>Tarea pendiente.</summary>
    Pending = 0,

    /// <summary>Tarea en progreso.</summary>
    InProgress = 1,

    /// <summary>Tarea completada.</summary>
    Completed = 2,

    /// <summary>Tarea cancelada.</summary>
    Cancelled = 3,
}
