using System.ComponentModel.DataAnnotations;

namespace StudioManager.API.Contracts.Pagination;

public sealed class PaginationDto
{
    private int? _limit;
    private int? _page;
    
    [Range(0, int.MaxValue)]
    public int? Limit
    {
        get => _limit;
        set => _limit = value ?? 10;
    }
    
    [Range(0, int.MaxValue)]
    public int? Page
    {
        get => _page;
        set => _page = value ?? 10;
    }

    public int GetOffset()
    {
        if (!Page.HasValue)
        {
            return 0;
        }
        
        return Limit!.Value * (Page.Value - 1);
    }
}