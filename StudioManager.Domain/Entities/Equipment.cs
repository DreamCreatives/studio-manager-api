namespace StudioManager.Domain.Entities;

public sealed class Equipment : EntityBase
{
    public string Name { get; private set; } = default!;
    public Guid EquipmentTypeId { get; private set; }
    public int InitialQuantity { get; private set; }
    public string ImageUrl { get; private set; } = default!;

    public static Equipment Create(string name, Guid equipmentTypeId, int quantity)
    {
        const string imagePlaceholder = "https://cdn3.iconfinder.com/data/icons/objects/512/equipment-512.png";
        return new Equipment
        {
            Id = Guid.NewGuid(),
            Name = name,
            EquipmentTypeId = equipmentTypeId,
            InitialQuantity = quantity,
            ImageUrl = imagePlaceholder
        };
    }

    public void Update(string name, Guid equipmentTypeId, int quantity)
    {
        Name = name;
        EquipmentTypeId = equipmentTypeId;
        InitialQuantity = quantity;
    }




    #region EntityRelations














    public EquipmentType EquipmentType { get; set; } = default!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();














    #endregion
}
