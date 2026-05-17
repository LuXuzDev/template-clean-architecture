using Domain.Specifications;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Specifications;

public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(
        IQueryable<T> inputQuery,
        Specification<T> spec)
    {
        var query = inputQuery;

        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        query = spec.Includes.Aggregate(query, 
            (current, include) => current.Include(include));

        query = spec.IncludeStrings.Aggregate(query, 
            (current, includeString) => current.Include(includeString));

        query = spec.IncludeExpressions.Aggregate(query, 
            (current, includeExpr) => includeExpr(current));

        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);

        // Paginación aplicada después del ordenamiento
        if (spec.Skip.HasValue)
            query = query.Skip(spec.Skip.Value);

        if (spec.Take.HasValue)
            query = query.Take(spec.Take.Value);

        if (spec.IsSplitQuery)
            query = query.AsSplitQuery();

        if (spec.IsNoTracking)
            query = query.AsNoTracking();

        return query;
    }
}