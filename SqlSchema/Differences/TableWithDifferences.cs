using SqlSchema.ObjectTypes;

namespace SqlSchema.Differences;

public record TableWithDifferences<T> where T : Base
{
    public string Table { get; set; } = null!;
    public IList<string> MissingInSource { get; set; } = null!;
    public IList<string> MissingInTarget { get; set; } = null!;
    public List<Difference<T>> Different { get; set; } = null!;

    public static List<TableWithDifferences<T>> Compute(HashSet<Table> sourceTables, HashSet<Table> targetTables)
    {
        var presentInBoth = sourceTables.Intersect(targetTables).ToList();

        var result = presentInBoth.Select(table =>
            {
                var sourceTable = sourceTables.First(x => x == table);
                var targetTable = targetTables.First(x => x == table);

                var sourceMember = sourceTable.GetMember<T>();
                var targetMember = targetTable.GetMember<T>();

                var sourceNames = sourceMember.Select(x => x.Name).ToList();
                var targetNames = targetMember.Select(x => x.Name).ToList();

                var tableWithDifferences = new TableWithDifferences<T>
                {
                    Table = table.ToString(),
                    MissingInSource = targetNames.Except(sourceNames).ToList(),
                    MissingInTarget = sourceNames.Except(targetNames).ToList(),
                    Different = Difference<T>.Compute(sourceMember, targetMember),
                };
                return tableWithDifferences;
            })
            .Where(x =>
                x.MissingInTarget.Count > 0 ||
                x.MissingInSource.Count > 0 ||
                x.Different.Count > 0)
            .ToList();

        return result;
    }
}