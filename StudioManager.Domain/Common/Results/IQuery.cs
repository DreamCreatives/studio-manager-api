using MediatR;

namespace StudioManager.Domain.Common.Results;

public interface IQuery<T> : IRequest<QueryResult<T>>;
