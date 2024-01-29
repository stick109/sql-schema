using Newtonsoft.Json;
using SqlSchema.Differences;
using SqlSchema.ObjectTypes;
using SqlSchema.Options;

namespace SqlSchema;

using Index = ObjectTypes.Index;

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

        string json;
        var tablesWithColumnDifferences = TableWithDifferences<Column>.Compute(sourceTables, targetTables);
        var tablesWithIndexDifferences = TableWithDifferences<Index>.Compute(sourceTables, targetTables);
        if (options.DetailedOutput)
        {
            var diff = new
            {
                TablesMissingInSource = missingInSource,
                TablesMissingInTarget = missingInTarget,
                TablesWithColumnDifferences = tablesWithColumnDifferences,
                TablesWithIndexDifferences = tablesWithIndexDifferences,
            };
            json = JsonConvert.SerializeObject(diff, Formatting.Indented);
        }
        else
        {
            string Format(Table x) => $"{x.Schema}.{x.Name}";
            var diff = new
            {
                TablesMissingInSource = missingInSource.Select(Format),
                TablesMissingInTarget = missingInTarget.Select(Format),
                TablesWithColumnDifferences = tablesWithColumnDifferences,
                TablesWithIndexDifferences = tablesWithIndexDifferences,
            };
            json = JsonConvert.SerializeObject(diff, Formatting.Indented);
        }
        Console.WriteLine(json);

        var indexesMissingInSource = tablesWithIndexDifferences.SelectMany(x => x.MissingInSource).Count();
        var indexesMissingInTarget = tablesWithIndexDifferences.SelectMany(x => x.MissingInTarget).Count();
        var indexesWithDifferences = tablesWithIndexDifferences.SelectMany(x => x.Different).Count();

        if (options.Verbose)
        {
            var statistics = new
            {
                TablesMissingInSource = missingInSource.Count,
                TablesMissingInTarget = missingInTarget.Count,
                TablesWithColumnDifferences = tablesWithColumnDifferences.Count,
                TablesWithIndexDifferences = tablesWithIndexDifferences.Count,
                IndexesMissingInSource = indexesMissingInSource,
                IndexesMissingInTarget = indexesMissingInTarget,
                IndexesWithDifferences = indexesWithDifferences,
            };
            Console.Error.WriteLine("Statistics:");
            json = JsonConvert.SerializeObject(statistics, Formatting.Indented);
            Console.Error.WriteLine(json);
        }
    }
}