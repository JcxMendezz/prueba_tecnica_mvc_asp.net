namespace TaskManagementSystem.Web.Models.ViewModels;

/// <summary>
/// ViewModel para la lista de tareas con filtros y contadores.
/// </summary>
public class TaskListViewModel
{
    /// <summary>Gets or sets the list of tasks.</summary>
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
    public IEnumerable<TaskViewModel> Tasks { get; set; } = [];
#pragma warning restore SA1010 // Opening square brackets should be spaced correctly

    /// <summary>Gets or sets the applied filters.</summary>
    public TaskFilterViewModel Filter { get; set; } = new ();

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

    /// <summary>Gets a value indicating whether there are any tasks.</summary>
    public bool HasTasks => Tasks.Any();

    /// <summary>Gets a value indicating whether there are no tasks.</summary>
    public bool IsEmpty => !HasTasks;
}
