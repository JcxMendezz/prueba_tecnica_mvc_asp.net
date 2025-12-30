using System.Diagnostics.CodeAnalysis;

namespace TaskManagementSystem.Web.Helpers.Results;

/// <summary>
/// Representa el resultado de una operación con datos.
/// Implementa el patrón Result para manejo de errores sin excepciones.
/// </summary>
/// <typeparam name="T">Tipo de dato del resultado.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Factory pattern requires static methods")]
public class Result<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    private Result()
    {
    }

    /// <summary>Gets a value indicating whether the operation was successful.</summary>
    public bool IsSuccess { get; private init; }

    /// <summary>Gets a value indicating whether the operation failed.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>Gets the value of the result.</summary>
    public T? Value { get; private init; }

    /// <summary>Gets the error message if the operation failed.</summary>
    public string? ErrorMessage { get; private init; }

    /// <summary>Gets the success message if the operation succeeded.</summary>
    public string? Message { get; private init; }

    /// <summary>
    /// Creates a successful result with data.
    /// </summary>
    /// <param name="value">The result value.</param>
    /// <param name="message">Optional success message.</param>
    /// <returns>A successful result.</returns>
    public static Result<T> Success(T value, string? message = null)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value,
            Message = message,
        };
    }

    /// <summary>
    /// Creates a failed result with error message.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
        };
    }
}

/// <summary>
/// Representa el resultado de una operación sin datos de retorno.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    private Result()
    {
    }

    /// <summary>Gets a value indicating whether the operation was successful.</summary>
    public bool IsSuccess { get; private init; }

    /// <summary>Gets a value indicating whether the operation failed.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>Gets the error message if the operation failed.</summary>
    public string? ErrorMessage { get; private init; }

    /// <summary>Gets the success message if the operation succeeded.</summary>
    public string? Message { get; private init; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="message">Optional success message.</param>
    /// <returns>A successful result.</returns>
    public static Result Success(string? message = null)
    {
        return new Result { IsSuccess = true, Message = message };
    }

    /// <summary>
    /// Creates a failed result with error message.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static Result Failure(string errorMessage)
    {
        return new Result
        {
            IsSuccess = false,
            ErrorMessage = errorMessage,
        };
    }
}
