using System.ComponentModel.DataAnnotations;

namespace StudioManager.API.Contracts.Pagination;

public sealed class PaginationDto
{
    [Range(0, int.MaxValue)] public int Limit { get; private init; } = 25;

    [Range(0, int.MaxValue)] public int Page { get; private init; } = 1;

    public int GetOffset()
    {
        return Limit * (Page - 1);
    }

    public static PaginationDto Default()
    {
        return new PaginationDto { Limit = 25, Page = 1 };
    }
}