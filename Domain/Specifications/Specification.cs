using System.Linq.Expressions;


namespace Domain.Specifications;

public abstract class Specification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<Func<IQueryable<T>, IQueryable<T>>> IncludeExpressions { get; } = new();
    public List<string> IncludeStrings { get; } = new();

    public Expression<Func<T, object>>? OrderBy { get; protected set; }
    public Expression<Func<T, object>>? OrderByDescending { get; protected set; }

    public int? Take { get; private set; }
    public int? Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; } = false;
    
    public bool IsSplitQuery { get; protected set; }
    public bool IsNoTracking { get; protected set; }

    protected Specification(Expression<Func<T, bool>>? criteria = null) => Criteria = criteria;

    protected void AddInclude(Expression<Func<T, object>> include) => Includes.Add(include);
    protected void AddInclude(Func<IQueryable<T>, IQueryable<T>> includeExpression) => IncludeExpressions.Add(includeExpression);

    // Cambiado a public para permitir el uso en el Handler
    public void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;
    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc) => OrderByDescending = orderByDesc;
    protected void ApplySplitQuery() => IsSplitQuery = true;
    protected void ApplyNoTracking() => IsNoTracking = true;
}