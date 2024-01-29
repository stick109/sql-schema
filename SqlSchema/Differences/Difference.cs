using SqlSchema.ObjectTypes;

namespace SqlSchema.Differences;

public record Difference<T> where T : Base
{
    public string Name { get; set; } = null!;
    public T Source { get; set; } = null!;
    public T Target { get; set; } = null!;

    public static List<Difference<T>> Compute(IList<T> sourceList, IList<T> targetList)
    {
        var targetDictionary = targetList.ToDictionary(x => x.Name);
        var result = sourceList.Select(source =>
            {
                if (!targetDictionary.ContainsKey(source.Name)) return null;
                var target = targetDictionary[source.Name];
                if (target.Equals(source)) return null;
                return new Difference<T>
                {
                    Name = source.Name,
                    Source = source,
                    Target = target,
                };
            })
            .Where(x => x != null)
            .ToList();
        return result!;
    }
}