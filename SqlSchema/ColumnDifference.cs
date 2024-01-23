using SqlSchema.ObjectTypes;

namespace SqlSchema;

public record ColumnDifference
{
    public string Name { get; set; } = null!;
    public Column Source { get; set; } = null!;
    public Column Target { get; set; } = null!;


    public static List<ColumnDifference> Compute(IList<Column> source, IList<Column> target)
    {
        var targetColumns = target.ToDictionary(x => x.Name);
        var result = source.Select(sourceColumn =>
        {
            if (!targetColumns.ContainsKey(sourceColumn.Name)) return null;
            var targetColumn = targetColumns[sourceColumn.Name];
            if (targetColumn == sourceColumn) return null;
            return new ColumnDifference
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