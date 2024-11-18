using Backend.Core.Domain;
using Backend.Core.Specification;

namespace Backend.Database.Adapter.Context;

public interface IBackendDbContext
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);

    Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : EntityBase;

    Task<TEntity?> FirstOrDefaultAsync<TEntity>(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken)
        where TEntity : EntityBase;

    Task<IEnumerable<TEntity>> FilterAsync<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken)
        where TEntity : EntityBase;

    void Remove<TEntity>(TEntity entity) where TEntity : EntityBase;
}