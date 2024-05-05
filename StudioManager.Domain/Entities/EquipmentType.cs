namespace StudioManager.Domain.Entities;

public sealed class EquipmentType : EntityBase
{
    public string Name { get; private set; } = default!;
    
    #region EntityRelations
    
    public IQueryable<Equipment> Equipments { get; set; } = default!;

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