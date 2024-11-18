namespace Backend.Core.Specification;

public interface ISpecification<TEntity>
{
    IQueryable<TEntity> Apply(IQueryable<TEntity> query); 
}