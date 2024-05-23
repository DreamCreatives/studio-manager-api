using System.Diagnostics.CodeAnalysis;

namespace StudioManager.Domain.Common.Results;

[ExcludeFromCodeCoverage]
public sealed class CheckResult<T>
{
    private CheckResult()
    {
    }

    public bool Succeeded { get; private set; }
    public T Data { get; private set; } = default!;
    public CommandResult CommandResult { get; private set; } = default!;

    /// <summary>
    ///     Creates successful result with data and success command result (with empty data)
    /// </summary>
    /// <param name="data">data to return as Generic parameter</param>
    /// <returns>successful check with data of type T</returns>
    public static CheckResult<T> Success(T data)
    {
        return new CheckResult<T>
        {
            Succeeded = true,
            Data = data,
            CommandResult = CommandResult.Success()
        };
    }

    /// <summary>
    ///     Returns failed result with NotFound command result
    /// </summary>
    /// <param name="id">id of entity that was not found</param>
    /// <typeparam name="TEntity">Type of entity that was not found</typeparam>
    /// <returns>failed check with empty data and NotFound CommandResult</returns>
    public static CheckResult<T> NotFound<TEntity>(object? id = null)
    {
        return Fail(CommandResult.NotFound<TEntity>(id));
    }

    /// <summary>
    ///     Returns failed result with Conflict command result
    /// </summary>
    /// <param name="message">Message to pass to conflict command resukt</param>
    /// <returns>failed check with empty data and Conflict CommandResult</returns>
    public static CheckResult<T> Conflict(string message)
    {
        return Fail(CommandResult.Conflict(message));
    }

    private static CheckResult<T> Fail(CommandResult result)
    {
        return new CheckResult<T>
        {
            Succeeded = false,
            Data = default!,
            CommandResult = result
        };
    }
}

public sealed class CheckResult
{
    private CheckResult()
    {
    }

    public bool Succeeded { get; private set; }
    public CommandResult CommandResult { get; private set; } = default!;

    /// <summary>
    ///     Creates successful result with success command result
    /// </summary>
    /// <returns>successful check with success command result</returns>
    public static CheckResult Success()
    {
        return new CheckResult
        {
            Succeeded = true,
            CommandResult = CommandResult.Success()
        };
    }

    /// <summary>
    ///     Returns failed result with NotFound command result
    /// </summary>
    /// <param name="result">Command result to return</param>
    /// <returns>failed check with NotFound CommandResult</returns>
    public static CheckResult Fail(CommandResult result)
    {
        return new CheckResult
        {
            Succeeded = false,
            CommandResult = result
        };
    }
}
