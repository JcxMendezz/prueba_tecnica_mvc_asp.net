using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Web.Models.Enums;

namespace TaskManagementSystem.Web.Models.ViewModels;

/// <summary>
/// ViewModel para editar una tarea.
/// </summary>
public class TaskEditViewModel
{
    /// <summary>Gets or sets the task ID.</summary>
    [Required]
    public int Id { get; set; }

    /// <summary>Gets or sets the task title.</summary>
    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
    [Display(Name = "Título")]
    public string Title { get; set; } = string.Empty;

    /// <summary>Gets or sets the task description.</summary>
    [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    /// <summary>Gets or sets the task status.</summary>
    [Required(ErrorMessage = "El estado es obligatorio")]
    [Display(Name = "Estado")]
    public TaskItemStatus Status { get; set; }

    /// <summary>Gets or sets the task priority.</summary>
    [Required(ErrorMessage = "La prioridad es obligatoria")]
    [Display(Name = "Prioridad")]
    public TaskPriority Priority { get; set; }

    /// <summary>Gets or sets the due date.</summary>
    [Display(Name = "Fecha de vencimiento")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    /// <summary>Gets or sets the creation date (read-only for display).</summary>
    [Display(Name = "Fecha de creación")]
    public DateTime CreatedAt { get; set; }
}
