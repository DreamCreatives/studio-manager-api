using MediatR;

namespace StudioManager.Domain.Common.Results;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, QueryResult<TResult>>
    where TQuery : IRequest<QueryResult<TResult>>;
