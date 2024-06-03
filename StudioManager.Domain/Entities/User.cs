namespace StudioManager.Domain.Entities;

public sealed class User : EntityBase
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public Guid KeycloakId { get; private set; }

    public string FullName => $"{FirstName} {LastName}";
    
    public static User Create(string firstName, string lastName, string email, Guid keycloakId)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            KeycloakId = keycloakId
        };
    }
    
    public void Update(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    #region EntityRelations

    public ICollection<Reservation> Reservations { get; private set; } = [];

    #endregion
}
