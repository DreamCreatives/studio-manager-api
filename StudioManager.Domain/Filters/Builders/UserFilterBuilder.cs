namespace StudioManager.Domain.Filters.Builders;

public class UserFilterBuilder
{
    private Guid? _id;
    private string? _email;
    private Guid? _notId;
    private string? _search;

    public static UserFilterBuilder New() => new();
    
    public UserFilterBuilder WithEmail(string? email)
    {
        _email = email;
        return this;
    }
    
    public UserFilterBuilder WithNotId(Guid? notId)
    {
        _notId = notId;
        return this;
    }
    
    public UserFilterBuilder WithSearch(string? search)
    {
        _search = search;
        return this;
    }

    public UserFilterBuilder WithId(Guid? id)
    {
        _id = id;
        return this;
    }
    
    public UserFilter Build()
    {
        return new UserFilter
        {
            Id = _id,
            Email = _email,
            NotId = _notId,
            Search = _search
        };
    }
}
