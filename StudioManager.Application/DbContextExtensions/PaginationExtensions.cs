using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Pagination;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.DbContextExtensions;

[ExcludeFromCodeCoverage]
public static class PaginationExtensions
{
    private static IQueryable<T> ApplyPaging<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
    {
        return paginationDto.Limit == 0
            ? queryable
            : queryable.Skip(paginationDto.GetOffset()).Take(paginationDto.Limit);
    }

    public static async Task<QueryResult<PagingResultDto<T>>> ApplyPagingAsync<T>(
        this IQueryable<T> queryable,
        PaginationDto pagination)
    {
        var data = await queryable.ApplyPaging(pagination).ToListAsync();
        var count = await queryable.CountAsync();

        return CreateResult(data, count, pagination);
    }

    private static QueryResult<PagingResultDto<T>> CreateResult<T>(List<T> data, int count, PaginationDto pagination)
    {
        return QueryResult.Success(new PagingResultDto<T>
        {
            Data = data,
            Pagination = new PaginationDetailsDto
            {
                Limit = pagination.Limit,
                Page = pagination.Page,
                Total = count
            }
        });
    }
}