using StudioManager.Domain.ErrorMessages;

namespace StudioManager.Domain.Entities;

public sealed class Equipment : EntityBase
{
    public string Name { get; private set; } = default!;
    public Guid EquipmentTypeId { get; private set; }
    public int Quantity { get; private set; }
    public int InitialQuantity { get; private set; }
    public string ImageUrl { get; private set; } = default!;


    #region EntityRelations
    
    public EquipmentType EquipmentType { get; set; } = default!;

    #endregion
    
    public void Reserve(int quantity)
    {
        Quantity -= quantity;
        if (Quantity < 0)
        {
            throw new InvalidOperationException(EX.EQUIPMENT_NEGATIVE_QUANTITY);
        }
    }
    
    public static Equipment Create(string name, Guid equipmentTypeId, int quantity)
    {
        const string imagePlaceholder = "https://cdn3.iconfinder.com/data/icons/objects/512/equipment-512.png";
        return new Equipment
        {
            Id = Guid.NewGuid(),
            Name = name,
            EquipmentTypeId = equipmentTypeId,
            InitialQuantity = quantity,
            Quantity = quantity,
            ImageUrl = imagePlaceholder
        };
    }
    
    public void Update(string name, Guid equipmentTypeId, int quantity)
    {
        Name = name;
        EquipmentTypeId = equipmentTypeId;
        Quantity = quantity;
        InitialQuantity = quantity;
    }
}