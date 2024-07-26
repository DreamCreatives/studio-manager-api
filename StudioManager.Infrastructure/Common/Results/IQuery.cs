using MediatR;

namespace StudioManager.Infrastructure.Common.Results;

public interface IQuery<T> : IRequest<QueryResult<T>>;
