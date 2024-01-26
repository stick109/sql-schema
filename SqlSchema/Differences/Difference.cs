using SqlSchema.ObjectTypes;

namespace SqlSchema.Differences;

public record Difference<T> where T : Base
{
    public string Name { get; set; } = null!;
    public T Source { get; set; } = null!;
    public T Target { get; set; } = null!;

    public static List<Difference<T>> Compute(IList<T> source, IList<T> target)
    {
        var targetColumns = target.ToDictionary(x => x.Name);
        var result = source.Select(sourceColumn =>
            {
                if (!targetColumns.ContainsKey(sourceColumn.Name)) return null;
                var targetColumn = targetColumns[sourceColumn.Name];
                if (targetColumn == sourceColumn) return null;
                return new Difference<T>
                {
                    Name = sourceColumn.Name,
                    Source = sourceColumn,
                    Target = targetColumn,
                };
            })
            .Where(x => x != null)
            .ToList();
        return result!;
    }
}