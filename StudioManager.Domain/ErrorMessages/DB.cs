// ReSharper disable InconsistentNaming

namespace StudioManager.Domain.ErrorMessages;

public static class DB
{
    public const string HAS_OPEN_TRANSACTION = "Transaction is already open";
    public const string NO_OPEN_TRANSACTION = "Transaction has not been started";




    #region EquipmentType














    public const string EQUIPMENT_TYPE_DUPLICATE_NAME = "Equipment type with this name already exists";














    #endregion




    #region Reservations














    public const string RESERVATION_EQUIPMENT_NOT_FOUND = "Cannot create reservation for equipment that does not exist";

    public const string RESERVATION_EQUIPMENT_QUANTITY_INSUFFICIENT =
        "Cannot create reservation, the quantity requested is greater than the available quantity";

    public const string RESERVATION_EQUIPMENT_USED_BY_OTHERS_IN_PERIOD =
        "Cannot create reservation, the equipment is already reserved by others in the specified period";














    #endregion
}

public static class DB_FORMAT
{
    #region Equipment














    public const string EQUIPMENT_QUANTITY_MISSING_WHEN_REMOVING =
        "Cannot remove equipment, the initial count ({0}) is not equal to the current count ({1})";

    public const string EQUIPMENT_DUPLICATE_NAME_TYPE = "Equipment with name {0} and type {1} already exists";
    public const string EQUIPMENT_DOES_NOT_EXIST = "[NOT FOUND] Equipment with id '{0}' does not exist";














    #endregion
    #region Reservations














    public const string RESERVATION_NOT_FOUND = "[NOT FOUND] Reservation with id '{0}' does not exist";














    #endregion




    #region EquipmentTypes














    public const string EQUIPMENT_TYPE_NOT_FOUND = "[NOT FOUND] EquipmentType with id '{0}' does not exist";














    #endregion
}
