namespace StudioManager.API.Contracts.Pagination;

public sealed class PagingResultDto<TData>
{
    public List<TData> Data { get; set; } = default!;
    public PaginationDetailsDto Pagination { get; set; } = default!;
}