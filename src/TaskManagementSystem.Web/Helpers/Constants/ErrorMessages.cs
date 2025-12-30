namespace TaskManagementSystem.Web.Helpers.Constants;

/// <summary>
/// Mensajes de error constantes para la aplicación.
/// Centraliza todos los textos para fácil mantenimiento e internacionalización.
/// </summary>
public static class ErrorMessages
{
    // Validación
    public const string TitleRequired = "El título es obligatorio";
    public const string TitleLength = "El título debe tener entre 3 y 200 caracteres";
    public const string TitleTooLong = "El título no puede exceder 200 caracteres";
    public const string DescriptionLength = "La descripción no puede exceder 1000 caracteres";
    public const string DescriptionTooLong = "La descripción no puede exceder 2000 caracteres";
    public const string StatusRequired = "El estado es obligatorio";
    public const string PriorityRequired = "La prioridad es obligatoria";
    public const string InvalidDate = "La fecha no es válida";
    public const string InvalidTaskId = "El ID de la tarea no es válido";

    // Operaciones de tareas
    public const string TaskNotFound = "La tarea no fue encontrada";
    public const string CreateError = "Error al crear la tarea";
    public const string UpdateError = "Error al actualizar la tarea";
    public const string DeleteError = "Error al eliminar la tarea";
    public const string TaskLoadError = "Error al cargar las tareas";

    // Base de datos
    public const string DatabaseError = "Error de conexión con la base de datos";
    public const string DatabaseTimeout = "La operación tardó demasiado tiempo";

    // General
    public const string UnexpectedError = "Ha ocurrido un error inesperado. Por favor, intente nuevamente.";
    public const string InternalError = "Ha ocurrido un error interno. Por favor, intente nuevamente.";
    public const string UnauthorizedError = "No tiene permisos para realizar esta acción";
}
