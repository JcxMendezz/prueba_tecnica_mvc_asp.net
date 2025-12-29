using Dapper;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models.Entities;
using TaskManagementSystem.Web.Models.ViewModels;
using TaskManagementSystem.Web.Repositories.Interfaces;

namespace TaskManagementSystem.Web.Repositories;

/// <summary>
/// Implementación del repositorio de tareas usando Dapper.
/// Maneja todas las operaciones de acceso a datos para tareas.
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly IDbConnectionFactory connectionFactory;
    private readonly ILogger<TaskRepository> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskRepository"/> class.
    /// </summary>
    /// <param name="connectionFactory">The database connection factory.</param>
    /// <param name="logger">The logger instance.</param>
    public TaskRepository(IDbConnectionFactory connectionFactory, ILogger<TaskRepository> logger)
    {
        this.connectionFactory = connectionFactory;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TaskEntity>> GetAllAsync(TaskFilterViewModel? filter = null)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            var (sql, parameters) = BuildSelectQuery(filter);

            logger.LogDebug("Executing query: {Query}", sql);
            return await connection.QueryAsync<TaskEntity>(sql, parameters);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all tasks");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<TaskEntity?> GetByIdAsync(int id)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = """
                SELECT id, title, description, status, priority,
                       due_date AS DueDate, created_at AS CreatedAt,
                       updated_at AS UpdatedAt, is_deleted AS IsDeleted
                FROM tasks
                WHERE id = @Id AND is_deleted = FALSE
                """;

            return await connection.QueryFirstOrDefaultAsync<TaskEntity>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting task by ID: {TaskId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<TaskEntity> CreateAsync(TaskEntity entity)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = """
                INSERT INTO tasks (title, description, status, priority, due_date, created_at, updated_at, is_deleted)
                VALUES (@Title, @Description, @Status, @Priority, @DueDate, @CreatedAt, @UpdatedAt, FALSE)
                RETURNING id, title, description, status, priority,
                          due_date AS DueDate, created_at AS CreatedAt,
                          updated_at AS UpdatedAt, is_deleted AS IsDeleted
                """;

            var created = await connection.QueryFirstAsync<TaskEntity>(sql, entity);
            logger.LogInformation("Task created with ID: {TaskId}", created.Id);
            return created;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating task");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(TaskEntity entity)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = """
                UPDATE tasks
                SET title = @Title,
                    description = @Description,
                    status = @Status,
                    priority = @Priority,
                    due_date = @DueDate,
                    updated_at = @UpdatedAt
                WHERE id = @Id AND is_deleted = FALSE
                """;

            var affected = await connection.ExecuteAsync(sql, entity);
            logger.LogInformation("Task updated: {TaskId}, Affected rows: {Rows}", entity.Id, affected);
            return affected > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating task: {TaskId}", entity.Id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();

            // Soft delete - solo marca como eliminado
            const string sql = """
                UPDATE tasks
                SET is_deleted = TRUE, updated_at = CURRENT_TIMESTAMP
                WHERE id = @Id AND is_deleted = FALSE
                """;

            var affected = await connection.ExecuteAsync(sql, new { Id = id });
            logger.LogInformation("Task soft deleted: {TaskId}, Affected rows: {Rows}", id, affected);
            return affected > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting task: {TaskId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = "SELECT EXISTS(SELECT 1 FROM tasks WHERE id = @Id AND is_deleted = FALSE)";
            return await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking if task exists: {TaskId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Dictionary<string, int>> GetCountByStatusAsync()
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = """
                SELECT
                    COALESCE(SUM(CASE WHEN status = 'Pending' THEN 1 ELSE 0 END), 0) AS Pending,
                    COALESCE(SUM(CASE WHEN status = 'InProgress' THEN 1 ELSE 0 END), 0) AS InProgress,
                    COALESCE(SUM(CASE WHEN status = 'Completed' THEN 1 ELSE 0 END), 0) AS Completed,
                    COALESCE(SUM(CASE WHEN status = 'Cancelled' THEN 1 ELSE 0 END), 0) AS Cancelled,
                    COALESCE(SUM(CASE WHEN due_date < CURRENT_DATE AND status NOT IN ('Completed', 'Cancelled') THEN 1 ELSE 0 END), 0) AS Overdue
                FROM tasks
                WHERE is_deleted = FALSE
                """;

            var result = await connection.QueryFirstAsync(sql);
            return new Dictionary<string, int>
            {
                ["Pending"] = (int)result.pending,
                ["InProgress"] = (int)result.inprogress,
                ["Completed"] = (int)result.completed,
                ["Cancelled"] = (int)result.cancelled,
                ["Overdue"] = (int)result.overdue,
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting task count by status");
            throw;
        }
    }

    private static (string Sql, DynamicParameters Parameters) BuildSelectQuery(TaskFilterViewModel? filter)
    {
        var parameters = new DynamicParameters();
        var sql = """
            SELECT id, title, description, status, priority,
                   due_date AS DueDate, created_at AS CreatedAt,
                   updated_at AS UpdatedAt, is_deleted AS IsDeleted
            FROM tasks
            WHERE is_deleted = FALSE
            """;

        if (filter is null)
        {
            return (sql + " ORDER BY created_at DESC", parameters);
        }

        // Filtro por estado
        if (filter.Status.HasValue)
        {
            sql += " AND status = @Status";
            parameters.Add("Status", filter.Status.Value.ToString());
        }

        // Filtro por prioridad
        if (filter.Priority.HasValue)
        {
            sql += " AND priority = @Priority";
            parameters.Add("Priority", filter.Priority.Value.ToString());
        }

        // Búsqueda por texto
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            sql += " AND (title ILIKE @Search OR description ILIKE @Search)";
            parameters.Add("Search", $"%{filter.SearchTerm.Trim()}%");
        }

        // Ordenamiento
        var orderColumn = filter.SortBy?.ToLowerInvariant() switch
        {
            "title" => "title",
            "status" => "status",
            "priority" => "priority",
            "duedate" => "due_date",
            _ => "created_at",
        };

        var direction = filter.SortDescending ? "DESC" : "ASC";
        sql += $" ORDER BY {orderColumn} {direction}";

        return (sql, parameters);
    }
}
