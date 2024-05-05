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
    
    public const string EQUIPMENT_DUPLICATE_NAME_TYPE = "Equipment with name {0} and type {1} already exists";
    
    #endregion
}