using Backend.Core.Domain;
using Backend.Core.Specification;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Backend.Database.Adapter.Context
{
    internal class BackendDbContext : IdentityDbContext, IBackendDbContext
    {

        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            var rowsAffects = await base.SaveChangesAsync(cancellationToken);

            return rowsAffects > 0;
        }

        public new async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : EntityBase => await this.Set<TEntity>().AddAsync(entity, cancellationToken);

        public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken) where TEntity : EntityBase
        {
            var query = BuildQuery(specification);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> FilterAsync<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken) where TEntity : EntityBase
        {
            var query = BuildQuery(specification);
            return await query.ToListAsync(cancellationToken);
        }

        public new void Remove<TEntity>(TEntity entity) where TEntity : EntityBase => Set<TEntity>().Remove(entity);

        private IQueryable<TEntity> BuildQuery<TEntity>(ISpecification<TEntity> specification) where TEntity : EntityBase
        {
            IQueryable<TEntity> query = this.Set<TEntity>();

            query = specification.Apply(query);

            return query;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                FilterEntityConfiguration
            );

            var properties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p =>
                    (p.ClrType == typeof(string) || p.ClrType == typeof(char) || p.ClrType == typeof(string[]) || p.ClrType.IsEnum) &&
                    p.GetColumnType() == null
                );

            foreach (var property in properties)
                property.SetIsUnicode(false);
        }

        bool FilterEntityConfiguration(Type type)
        {
            var isEntityConfiguration = type.GetInterfaces().Any(interfaceType =>
                interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
            );

            if (!isEntityConfiguration) return false;

            if (type.Namespace?.StartsWith($"{GetType().Namespace}.Configurations.ProviderCustomization") ?? false)
            {
                var provider = (Database.ProviderName ?? string.Empty).Split('.').Last();
                return type.Namespace.Split('.').Last() == provider;
            }

            return true;
        }

    }
}
