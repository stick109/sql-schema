using Newtonsoft.Json;
using SqlSchema.ObjectTypes;
using SqlSchema.Options;

namespace SqlSchema;

public class Comparer
{
    public static void Run(Compare options)
    {
        options.Print();

        var sourceJson = File.ReadAllText(options.Source);
        var source = JsonConvert.DeserializeObject<Schema>(sourceJson) ?? throw new Exception($"Unable to deserialize {options.Source}");
        var targetJson = File.ReadAllText(options.Target);
        var target = JsonConvert.DeserializeObject<Schema>(targetJson) ?? throw new Exception($"Unable to deserialize {options.Target}");

        HashSet<Table> sourceTables = source.Tables.ToHashSet();
        HashSet<Table> targetTables = target.Tables.ToHashSet();

        var missingInSource = targetTables.Except(sourceTables).ToList();
        var missingInTarget = sourceTables.Except(targetTables).ToList();
        var presentInBoth = sourceTables.Intersect(targetTables).ToList();

        List<TableColumnDifference> CompareColumns()
        {
            var columnDifferences = presentInBoth!.Select(table =>
            {
                var sourceTable = sourceTables.First(x => x == table);
                var targetTable = targetTables.First(x => x == table);
                var sourceColumnNames = sourceTable.Columns.Select(x => x.Name).ToList();
                var targetColumnNames = targetTable.Columns.Select(x => x.Name).ToList();

                var columnDifference = new TableColumnDifference
                {
                    Table = table.Name,
                    MissingInSource = targetColumnNames.Except(sourceColumnNames).ToList(),
                    MissingInTarget = sourceColumnNames.Except(targetColumnNames).ToList(),
                    Different = ColumnDifference.Compute(sourceTable.Columns, targetTable.Columns),
                };
                return columnDifference;
            })
                .ToList();

            return columnDifferences;
        }

        string BuildDiffJson()
        {
            if (options.DetailedOutput)
            {
                var diff = new
                {
                    TablesMissingInSource = missingInSource,
                    TablesMissingInTarget = missingInTarget,
                };
                return JsonConvert.SerializeObject(diff, Formatting.Indented);
            }
            else
            {
                string Format(Table x) => $"{x.Schema}.{x.Name}";
                var diff = new
                {
                    TablesMissingInSource = missingInSource.Select(Format),
                    TablesMissingInTarget = missingInTarget.Select(Format),
                    TablesWithColumnDifferences = CompareColumns(),
                };
                return JsonConvert.SerializeObject(diff, Formatting.Indented);
            }
        }

        var json = BuildDiffJson();
        Console.WriteLine(json);

        if (options.Verbose)
        {
            var statistics = new
            {
                TablesMissingInSource = missingInSource.Count,
                TablesMissingInTarget = missingInTarget.Count,
                TablesPresentInBoth = presentInBoth.Count,
            };
            Console.Error.WriteLine("Statistics:");
            json = JsonConvert.SerializeObject(statistics, Formatting.Indented);
            Console.Error.WriteLine(json);
        }
    }
}