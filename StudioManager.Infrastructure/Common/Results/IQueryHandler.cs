using MediatR;

namespace StudioManager.Infrastructure.Common.Results;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, QueryResult<TResult>>
    where TQuery : IRequest<QueryResult<TResult>>;
