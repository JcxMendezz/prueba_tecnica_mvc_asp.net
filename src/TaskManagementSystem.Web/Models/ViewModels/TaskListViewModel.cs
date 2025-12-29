namespace TaskManagementSystem.Web.Models.ViewModels;

/// <summary>
/// ViewModel para la lista de tareas con filtros.
/// </summary>
public class TaskListViewModel
{
    /// <summary>Gets or sets the list of tasks.</summary>
    public IEnumerable<TaskViewModel> Tasks { get; set; } = new List<TaskViewModel>();

    /// <summary>Gets or sets the applied filters.</summary>
    public TaskFilterViewModel Filter { get; set; } = new TaskFilterViewModel();

    /// <summary>Gets or sets the total count of tasks.</summary>
    public int TotalCount { get; set; }

    /// <summary>Gets or sets the count of pending tasks.</summary>
    public int PendingCount { get; set; }

    /// <summary>Gets or sets the count of in-progress tasks.</summary>
    public int InProgressCount { get; set; }

    /// <summary>Gets or sets the count of completed tasks.</summary>
    public int CompletedCount { get; set; }

    /// <summary>Gets or sets the count of overdue tasks.</summary>
    public int OverdueCount { get; set; }
}
