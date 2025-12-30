using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Web.Models.Enums;

namespace TaskManagementSystem.Web.Models.ViewModels;

/// <summary>
/// ViewModel para crear una tarea.
/// </summary>
public class TaskCreateViewModel
{
    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
    [Display(Name = "Título")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio")]
    [Display(Name = "Estado")]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;

    [Required(ErrorMessage = "La prioridad es obligatoria")]
    [Display(Name = "Prioridad")]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    [Display(Name = "Fecha de vencimiento")]
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }
}
