using Newtonsoft.Json;

namespace SqlSchema;

public class SchemaComparer
{
    public static void Run(string sourceSchemaFile, string targetSchemaFile)
    {
        var sourceJson = File.ReadAllText(sourceSchemaFile);
        var source = JsonConvert.DeserializeObject<Schema>(sourceJson) ?? throw new Exception($"Unable to deserialize {sourceSchemaFile}");
        var targetJson = File.ReadAllText(targetSchemaFile);
        var target = JsonConvert.DeserializeObject<Schema>(targetJson) ?? throw new Exception($"Unable to deserialize {targetSchemaFile}"); ;
        var sourceTables = source.Tables.ToHashSet();
        var targetTables = target.Tables.ToHashSet();
        var missingInSource = targetTables.Except(sourceTables);
        var missingInTarget = sourceTables.Except(targetTables);
        var diff = new
        {
            TablesMissingInSource = missingInSource,
            TablesMissingInTarget = missingInTarget,
        };
        var json = JsonConvert.SerializeObject(diff, Formatting.Indented);
        Console.Error.WriteLine(json);
    }
}