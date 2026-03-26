using System.Linq.Expressions;


namespace Domain.Specifications;

public class AndSpecification<T> : Specification<T>
{
    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        Criteria = Combine(left.Criteria, right.Criteria, Expression.AndAlso);

        foreach (var include in left.Includes)
            Includes.Add(include);

        foreach (var include in right.Includes)
            Includes.Add(include);
    }

    private static Expression<Func<T, bool>> Combine(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        Func<Expression, Expression, Expression> merge)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceParameterVisitor(left.Parameters[0], parameter);
        var leftBody = leftVisitor.Visit(left.Body);

        var rightVisitor = new ReplaceParameterVisitor(right.Parameters[0], parameter);
        var rightBody = rightVisitor.Visit(right.Body);

        return Expression.Lambda<Func<T, bool>>(
            merge(leftBody!, rightBody!),
            parameter);
    }
}