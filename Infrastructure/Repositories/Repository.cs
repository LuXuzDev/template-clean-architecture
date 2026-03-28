using Domain.Shared.Abstractions;
using Domain.Specifications;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _dbContext;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<T>> ListAsync(CancellationToken ct, Specification<T>? spec = null)
    {
        if(spec is not null)
        {
            var query = SpecificationEvaluator<T>.GetQuery(
            _dbContext.Set<T>().AsQueryable(), spec);

            return await query.ToListAsync(ct);
        }
        else
            return await _dbContext.Set<T>().AsQueryable().ToListAsync(ct);
        
    }

    public async Task<T?> FirstOrDefaultAsync(Specification<T> spec, CancellationToken ct)
    {
        var query = SpecificationEvaluator<T>.GetQuery(
            _dbContext.Set<T>().AsQueryable(), spec);

        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<bool> AnyAsync(Specification<T> spec, CancellationToken ct)
    {
        var query = SpecificationEvaluator<T>.GetQuery(
            _dbContext.Set<T>().AsQueryable(), spec);

        return await query.AnyAsync(ct);
    }


    public async Task CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
    }

    public async Task UpdateAsync(T entity, CancellationToken ct)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public async Task DeleteAsync(T entity, CancellationToken ct)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public async Task SoftDeleteAsync(T entity, CancellationToken ct)
    {
        entity.IsDeleted = true;

        _dbContext.Set<T>().Update(entity);
    }

    public async Task<int> TotalCount(CancellationToken ct, Specification<T>? spec = null)
    {
        if(spec is not null)
        {
            var query = SpecificationEvaluator<T>.GetQuery(
            _dbContext.Set<T>().AsQueryable(), spec);

            return await query.CountAsync(ct);
        }
        else
            return await _dbContext.Set<T>().AsQueryable().CountAsync(ct);
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken ct)
    {
        var entitiesList = entities.ToList();

        if (entitiesList.Any())
        {
            _dbContext.Set<T>().RemoveRange(entitiesList);
        }

        return Task.CompletedTask;
    }

    public Task SoftDeleteRangeAsync(IEnumerable<T> entities, CancellationToken ct)
    {
        var entitiesList = entities.ToList();

        foreach (var entity in entitiesList)
        {
            entity.IsDeleted = true;
        }

        if (entitiesList.Any())
        {
            _dbContext.Set<T>().UpdateRange(entitiesList);
        }

        return Task.CompletedTask;
    }

    public async Task<int> DeleteBySpecAsync(Specification<T> spec, CancellationToken ct)
    {
        var query = SpecificationEvaluator<T>.GetQuery(
            _dbContext.Set<T>().AsQueryable(), spec);

        var entitiesToDelete = await query.ToListAsync(ct);

        if (entitiesToDelete.Any())
        {
            _dbContext.Set<T>().RemoveRange(entitiesToDelete);
        }

        return entitiesToDelete.Count;
    }


    public async Task<int> SoftDeleteBySpecAsync(Specification<T> spec, CancellationToken ct)
    {
        var query = SpecificationEvaluator<T>.GetQuery(
            _dbContext.Set<T>().AsQueryable(), spec);

        var entitiesToUpdate = await query.ToListAsync(ct);

        foreach (var entity in entitiesToUpdate)
        {
            entity.IsDeleted = true;
        }

        if (entitiesToUpdate.Any())
        {
            _dbContext.Set<T>().UpdateRange(entitiesToUpdate);
        }

        return entitiesToUpdate.Count;
    }
}