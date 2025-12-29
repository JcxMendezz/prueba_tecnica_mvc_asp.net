namespace TaskManagementSystem.Web.Models.Entities;

/// <summary>
/// Entidad que representa una tarea en la base de datos.
/// </summary>
public class TaskEntity
{
    /// <summary>Identificador único.</summary>
    public int Id { get; set; }

    /// <summary>Título de la tarea.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Descripción detallada.</summary>
    public string? Description { get; set; }

    /// <summary>Estado actual (como string para mapear con BD).</summary>
    public string Status { get; set; } = "Pending";

    /// <summary>Nivel de prioridad (como string para mapear con BD).</summary>
    public string Priority { get; set; } = "Medium";

    /// <summary>Fecha de vencimiento.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>Fecha de creación.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha de última actualización.</summary>
    public DateTime UpdatedAt { get; set; }
}
