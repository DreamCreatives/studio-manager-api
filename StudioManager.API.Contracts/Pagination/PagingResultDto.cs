namespace StudioManager.API.Contracts.Pagination;

public sealed class PagingResultDto<TData>
{
    public IReadOnlyList<TData> Data { get; set; } = default!;
    public PaginationDetailsDto Pagination { get; set; } = default!;
}