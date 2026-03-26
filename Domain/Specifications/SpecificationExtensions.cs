namespace Domain.Specifications;

public static class SpecificationExtensions
{
    public static Specification<T> And<T>(
        this Specification<T> left,
        Specification<T> right)
    {
        return new AndSpecification<T>(left, right);
    }

    public static Specification<T> Or<T>(
        this Specification<T> left,
        Specification<T> right)
    {
        return new OrSpecification<T>(left, right);
    }

    public static Specification<T> Not<T>(this Specification<T> spec)
    {
        return new NotSpecification<T>(spec);
    }
}