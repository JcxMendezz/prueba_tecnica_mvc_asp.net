namespace TaskManagementSystem.Web.Models.ViewModels;

/// <summary>
/// ViewModel para la lista de tareas con filtros, contadores y paginación.
/// </summary>
public class TaskListViewModel
{
    /// <summary>Gets or sets the list of tasks.</summary>
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
    public IEnumerable<TaskViewModel> Tasks { get; set; } = [];
#pragma warning restore SA1010 // Opening square brackets should be spaced correctly

    /// <summary>Gets or sets the applied filters.</summary>
    public TaskFilterViewModel Filter { get; set; } = new ();

    /// <summary>Gets or sets the total count of all tasks (without pagination).</summary>
    public int TotalCount { get; set; }

    /// <summary>Gets or sets the count of pending tasks.</summary>
    public int PendingCount { get; set; }

    /// <summary>Gets or sets the count of in-progress tasks.</summary>
    public int InProgressCount { get; set; }

    /// <summary>Gets or sets the count of completed tasks.</summary>
    public int CompletedCount { get; set; }

    /// <summary>Gets or sets the count of overdue tasks.</summary>
    public int OverdueCount { get; set; }

    /// <summary>Gets a value indicating whether there are any tasks.</summary>
    public bool HasTasks => Tasks.Any();

    /// <summary>Gets a value indicating whether there are no tasks.</summary>
    public bool IsEmpty => !HasTasks;

    // ===== Propiedades de Paginación =====

    /// <summary>Gets or sets current page number.</summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>Gets or sets page size.</summary>
    public int PageSize { get; set; } = 5;

    /// <summary>Gets or sets total number of filtered items.</summary>
    public int FilteredCount { get; set; }

    /// <summary>Gets total pages.</summary>
    public int TotalPages => (int)Math.Ceiling((double)FilteredCount / PageSize);

    /// <summary>Gets a value indicating whether has previous page.</summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>Gets a value indicating whether has next page.</summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>Gets start item number for display.</summary>
    public int StartItem => FilteredCount == 0 ? 0 : ((CurrentPage - 1) * PageSize) + 1;

    /// <summary>Gets end item number for display.</summary>
    public int EndItem => Math.Min(CurrentPage * PageSize, FilteredCount);
}
