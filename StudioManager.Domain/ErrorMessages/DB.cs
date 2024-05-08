// ReSharper disable InconsistentNaming
namespace StudioManager.Domain.ErrorMessages;

public static class DB
{
    public const string HAS_OPEN_TRANSACTION = "Transaction is already open";
    public const string NO_OPEN_TRANSACTION = "Transaction has not been started";
    
    #region EquipmentType
    
    public const string EQUIPMENT_TYPE_DUPLICATE_NAME = "Equipment type with this name already exists";
    
    #endregion
}

public static class DB_FORMAT
{
    #region Equipment
    
    public const string EQUIPMENT_QUANTITY_MISSING_WHEN_REMOVING = "Cannot remove equipment, the initial count ({0}) is not equal to the current count ({1})";
    public const string EQUIPMENT_DUPLICATE_NAME_TYPE = "Equipment with name {0} and type {1} already exists";
    
    #endregion
}