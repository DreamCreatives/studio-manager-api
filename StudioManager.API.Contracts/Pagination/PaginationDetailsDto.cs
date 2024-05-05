namespace StudioManager.API.Contracts.Pagination;

public sealed class PaginationDetailsDto
{
    public int Limit { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
}