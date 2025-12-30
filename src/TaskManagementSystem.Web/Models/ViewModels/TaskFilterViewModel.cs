using TaskManagementSystem.Web.Models.Enums;

namespace TaskManagementSystem.Web.Models.ViewModels;

/// <summary>
/// ViewModel para filtrar tareas.
/// </summary>
public class TaskFilterViewModel
{
    /// <summary>Gets or sets filtro por estado.</summary>
    public TaskItemStatus? Status { get; set; }

    /// <summary>Gets or sets filtro por prioridad.</summary>
    public TaskPriority? Priority { get; set; }

    /// <summary>Gets or sets término de búsqueda.</summary>
    public string? SearchTerm { get; set; }

    /// <summary>Gets or sets campo para ordenar.</summary>
    public string SortBy { get; set; } = "CreatedAt";

    /// <summary>Gets or sets a value indicating whether ordenar descendente.</summary>
    public bool SortDescending { get; set; } = true;

    /// <summary>Gets a value indicating whether indica si hay filtros activos.</summary>
    public bool HasActiveFilters => Status.HasValue || Priority.HasValue || !string.IsNullOrWhiteSpace(SearchTerm);
}
