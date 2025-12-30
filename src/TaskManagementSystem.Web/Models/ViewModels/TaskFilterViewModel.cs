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
    public string SortBy { get; set; } = "DueDate";

    /// <summary>Gets or sets a value indicating whether ordenar descendente.</summary>
    public bool SortDescending { get; set; }

    /// <summary>Gets or sets número de página actual.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Gets or sets tamaño de página.</summary>
    public int PageSize { get; set; } = 5;

    /// <summary>Gets a value indicating whether indica si hay filtros activos.</summary>
    public bool HasActiveFilters => Status.HasValue || Priority.HasValue || !string.IsNullOrWhiteSpace(SearchTerm);
}
