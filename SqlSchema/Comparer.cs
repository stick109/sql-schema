using Newtonsoft.Json;
using SqlSchema.ObjectTypes;
using SqlSchema.Options;

namespace SqlSchema;

public class Comparer
{
    public static void Run(Compare options)
    {
        options.Run();

        var sourceJson = File.ReadAllText(options.Source);
        var source = JsonConvert.DeserializeObject<Schema>(sourceJson) ?? throw new Exception($"Unable to deserialize {options.Source}");
        var targetJson = File.ReadAllText(options.Target);
        var target = JsonConvert.DeserializeObject<Schema>(targetJson) ?? throw new Exception($"Unable to deserialize {options.Target}"); ;
        var sourceTables = source.Tables.ToHashSet();
        var targetTables = target.Tables.ToHashSet();
        var missingInSource = targetTables.Except(sourceTables).ToList();
        var missingInTarget = sourceTables.Except(targetTables).ToList();
        var diff = new
        {
            TablesMissingInSource = missingInSource,
            TablesMissingInTarget = missingInTarget,
        };
        var json = JsonConvert.SerializeObject(diff, Formatting.Indented);
        Console.WriteLine(json);

        if (options.Verbose)
        {
            var statistics = new
            {
                MissingInSourceCount = missingInSource.Count,
                MissingInTargetCount = missingInTarget.Count,
            };
            Console.Error.WriteLine("Statistics:");
            json = JsonConvert.SerializeObject(statistics, Formatting.Indented);
            Console.Error.WriteLine(json);
        }
    }
}