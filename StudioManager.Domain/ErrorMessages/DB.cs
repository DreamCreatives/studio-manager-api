// ReSharper disable InconsistentNaming
namespace StudioManager.Domain.ErrorMessages;

public static class DB
{
    public const string HAS_OPEN_TRANSACTION = "Transaction is already open";
    public const string NO_OPEN_TRANSACTION = "Transaction has not been started";
}