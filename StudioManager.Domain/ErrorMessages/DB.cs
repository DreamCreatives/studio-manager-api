// ReSharper disable InconsistentNaming
namespace StudioManager.Domain.ErrorMessages;

public static class DB
{
    public const string HAS_OPEN_TRANSACTION = "Transaction is already open";
    public const string NO_OPEN_TRANSACTION = "Transaction has not been started";
    
    #region EquipmentType
    
    public const string EQUIPMENT_TYPE_NON_UNIQUE_NAME = "Equipment type with this name already exists";
    
    #endregion
    
}