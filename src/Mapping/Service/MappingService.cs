using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stize.Domain;
using Stize.DotNet.Delta;
using Stize.DotNet.Specification;
using Stize.Persistence.EntityFrameworkCore;

namespace Stize.Mapping.Service
{
    public class MappingService<TContext> : IMappingService<TContext>
        where TContext : DbContext
    {
        protected ILogger<MappingService<TContext>> Logger { get; }
        protected IObjectMapper Mapper { get; }

        private readonly IEntityRepository<TContext> repository;

        public MappingService(ILogger<MappingService<TContext>> logger, IEntityRepository<TContext> repository, IObjectMapper mapper)
        {
            this.Logger = logger;
            this.repository = repository;
            this.Mapper = mapper;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel, TEntity>(CancellationToken cancellationToken = default)
            where TModel : class
            where TEntity : class
        {
            var entities = await this.repository.GetAll<TEntity>().ToArrayAsync(cancellationToken);
            var models = this.Mapper.Map<TEntity, TModel>(entities);
            return models.ToArray();
        }

        public async Task RemoveAsync<TModel, TEntity>(TModel model, CancellationToken cancellationToken = default)
            where TModel : class
            where TEntity : class
        {
            var dest = this.Mapper.Map<TModel, TEntity>(model);
            this.repository.Remove(dest);
            await this.repository.CommitAsync(cancellationToken);
        }

        public async Task<IEnumerable<TModel>> WhereAsync<TModel, TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            where TModel : class
            where TEntity : class
        {
            var entities = await this.repository.Where(specification).ToArrayAsync(cancellationToken);
            var models = this.Mapper.Map<TEntity, TModel>(entities);
            return models.ToArray();
        }

        public async Task<bool> AnyAsync<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            //where TModel : class 
            where TEntity : class
        {
            return await this.repository.Where(specification).AnyAsync(cancellationToken);
        }

        public async Task<int> CountAsync<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            //where TModel : class 
            where TEntity : class
        {
            return await this.repository.Where(specification).CountAsync(cancellationToken);
        }

        public async Task<TKey> AddAsync<TModel, TEntity, TKey>(TModel model, CancellationToken cancellationToken = default)
            where TModel : class
            where TEntity : class, IObject<TKey>
        {
            var entity = this.Create<TEntity>();
            var dest = this.Mapper.Map(model, entity);
            this.repository.Add(dest);
            await this.repository.CommitAsync(cancellationToken);
            return entity.Id;
        }

        public async Task<TModel> FindOneAsync<TModel, TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
            where TModel : class, IObject<TKey>
            where TEntity : class, IObject<TKey>
        {
            var entity = await this.repository.FindOneAsync<TEntity, TKey>(id, cancellationToken);
            var model = this.Mapper.Map<TEntity, TModel>(entity);
            return model;
        }

        public async Task RemoveAsync<TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
            where TEntity : class, IObject<TKey>
        {
            await this.repository.RemoveAsync<TEntity, TKey>(id, cancellationToken);
            await this.repository.CommitAsync(cancellationToken);
        }

        public async Task ApplyChangesAsync<TModel, TEntity, TKey>(TModel model, CancellationToken cancellationToken = default)
            where TModel : class, IObject<TKey>
            where TEntity : class, IObject<TKey>
        {
            var entity = await this.repository.FindOneAsync<TEntity, TKey>(model.Id, cancellationToken);
            if (entity == null)
            {
                entity = this.Create<TEntity>();
                this.repository.Add(entity);
            }

            this.Mapper.Map(model, entity);

            await this.repository.CommitAsync(cancellationToken);
        }

        public async Task PatchAsync<TModel, TEntity, TKey>(Delta<TModel> delta, CancellationToken cancellationToken = default)
            where TModel : class, IObject<TKey>
            where TEntity : class, IObject<TKey>
        {
            var idName = nameof(IObject<TKey>.Id);
            if (delta.GetChangedPropertyNames().Contains(idName))
            {
                if (delta.TryGetPropertyValue(idName, out var value))
                {
                    if (value is TKey id)
                    {
                        var entity = await this.repository.FindOneAsync<TEntity, TKey>(id, cancellationToken);
                        if (entity != null)
                        {
                            var model = this.Mapper.Map<TEntity, TModel>(entity);
                            delta.Patch(model);
                            entity = this.Mapper.Map(model, entity);
                            await this.repository.CommitAsync(cancellationToken);
                        }

                        this.Logger.LogDebug($"The entity of type {typeof(TEntity).Name} with key {id} does not exits and can not be patched");
                    }
                    else
                    {
                        this.Logger.LogDebug($"The type of {value.GetType()} does not match the key type {typeof(TKey).Name} fot the entity type {typeof(TEntity).Name} and can not be patched");
                    }
                }
                else
                {
                    this.Logger.LogDebug($"The delta of entity of type {typeof(TEntity).Name} does not have a key ({idName}) and can not be patched");
                }
            }
            else
            {
                this.Logger.LogDebug($"The delta of entity of type {typeof(TEntity).Name} does not have a key property provided ({idName}) and can not be patched");
            }

        }

        private T Create<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
