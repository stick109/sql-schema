namespace SqlSchema;

public record Difference<T>
{
    public List<T> MissingInSource { get; private set; } = null!;
    public List<T> MissingInTarget { get; private set; } = null!;
    public List<T> PresentInBoth { get; private set; } = null!;

    public static Difference<T> Compute(IList<T> source, IList<T> target)
    {
        var result = new Difference<T>
        {
            MissingInSource = target.Except(source).ToList(),
            MissingInTarget = source.Except(target).ToList(),
            PresentInBoth = source.Intersect(target).ToList(),
        };
        return result;
    }
}