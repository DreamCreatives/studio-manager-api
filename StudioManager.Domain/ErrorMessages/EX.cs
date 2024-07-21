// ReSharper disable InconsistentNaming

namespace StudioManager.Domain.ErrorMessages;

public static class EX
{
    public const string SUCCESS_FROM_ERROR = "Cannot create success result from error";
    public const string ERROR_FROM_SUCCESS = "Cannot create error result from success";
    public const string FORBIDDEN = "You do not have permission to perform this action";
}
