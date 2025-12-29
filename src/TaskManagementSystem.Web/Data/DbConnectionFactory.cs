using System.Data;
using Npgsql;

namespace TaskManagementSystem.Web.Data;

/// <summary>
/// Factory para crear conexiones a la base de datos PostgreSQL.
/// Implementa el patrón Factory para centralizar la creación de conexiones.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Crea una nueva conexión a la base de datos.
    /// </summary>
    /// <returns>Una conexión a la base de datos.</returns>
    IDbConnection CreateConnection();
}

/// <summary>
/// Implementación de la factory de conexiones para PostgreSQL.
/// </summary>
public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbConnectionFactory"/> class.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    public DbConnectionFactory(string connectionString)
    {
        this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    /// <inheritdoc/>
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(this.connectionString);
    }
}
