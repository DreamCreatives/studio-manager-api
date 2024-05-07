namespace StudioManager.Domain.Entities;

public sealed class EquipmentType : EntityBase
{
    public string Name { get; private set; } = default!;
    
    #region EntityRelations
    
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

    #endregion
    
    public void Update(string name)
    {
        Name = name;
    }
    
    public static EquipmentType Create(string name)
    {
        return new EquipmentType
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}