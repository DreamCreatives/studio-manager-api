using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudioManager.API.Contracts.Pagination;
using StudioManager.Domain.Common.Results;

namespace StudioManager.Application.DbContextExtensions;

public static class PaginationExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
    {
        return paginationDto.Limit == 0
            ? queryable
            : queryable.Skip(paginationDto.GetOffset()).Take(paginationDto.Limit);
    }

    public static IEnumerable<T> ApplyPaging<T>(this IEnumerable<T> enumerable, PaginationDto paginationDto)
    {
        return paginationDto.Limit == 0
            ? enumerable
            : enumerable.Skip(paginationDto.GetOffset()).Take(paginationDto.Limit);
    }

    public static async Task<QueryResult<PagingResultDto<TDestination>>> ApplyPagingAndMapAsync<TSource, TDestination>(
        this IQueryable<TSource> queryable,
        PaginationDto pagination,
        IMapper mapper)
    {
        return await queryable.ApplyPagingAndMapAsync(pagination,
            x => Task.FromResult(mapper.Map<IEnumerable<TDestination>>(x)));
    }

    public static async Task<QueryResult<PagingResultDto<T>>> ApplyPagingAsync<T>(
        this IQueryable<T> queryable,
        PaginationDto pagination)
    {
        var data = await queryable.ApplyPaging(pagination).ToListAsync();
        var count = await queryable.CountAsync();

        return CreateResult(data, count, pagination);
    }

    public static async Task<QueryResult<PagingResultDto<TDestination>>> ApplyPagingAndMapAsync<TSource, TDestination>(
        this IQueryable<TSource> queryable,
        PaginationDto pagination,
        Func<IEnumerable<TSource>, Task<IEnumerable<TDestination>>> mappingFunc)
    {
        var data = await queryable.ApplyPaging(pagination).ToListAsync();
        var mappedData = await mappingFunc(data);
        var count = await queryable.CountAsync();

        return CreateResult(mappedData.ToList(), count, pagination);
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