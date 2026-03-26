using System.Linq.Expressions;


namespace Domain.Specifications;

public class NotSpecification<T> : Specification<T>
{
    public NotSpecification(Specification<T> spec)
    {
        if (spec.Criteria is null)
            throw new ArgumentNullException(nameof(spec.Criteria));

        var parameter = Expression.Parameter(typeof(T));

        var visitor = new ReplaceParameterVisitor(spec.Criteria.Parameters[0], parameter);
        var body = visitor.Visit(spec.Criteria.Body);

        Criteria = Expression.Lambda<Func<T, bool>>(
            Expression.Not(body!),
            parameter);

        foreach (var include in spec.Includes)
            Includes.Add(include);
    }
}