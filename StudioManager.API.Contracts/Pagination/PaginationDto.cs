using System.ComponentModel.DataAnnotations;

namespace StudioManager.API.Contracts.Pagination;

public sealed class PaginationDto
{
    public const int DefaultLimit = 50;
    public const int DefaultPage = 1;

    private int? _limit;
    private int? _page;

    [Range(0, int.MaxValue)]
    public int? Limit
    {
        get => _limit ?? DefaultLimit;
        set => _limit = value;
    }

    [Range(0, int.MaxValue)]
    public int? Page
    {
        get => _page ?? DefaultPage;
        set => _page = value;
    }

    public int GetOffset()
    {
        if (!Page.HasValue) return 0;

        return Limit!.Value * (Page!.Value - 1);
    }
}
