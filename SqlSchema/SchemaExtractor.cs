using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace SqlSchema;

public class SchemaExtractor
{
    public static void Run(string server, string database, bool verbose)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = server,
            InitialCatalog = database,
            IntegratedSecurity = true,
            TrustServerCertificate = true,
        };
        if (verbose)
        {
            Console.Error.WriteLine($"Connecting as [{builder.ConnectionString}]");
        }

        using var connection = new SqlConnection(builder.ConnectionString);
        var sql = "select s.name as [schema], t.* from sys.tables t join sys.schemas s on s.schema_id = t.schema_id";
        var tables = connection.Query<Table>(sql).ToList();
        var tablesDictionary = tables.ToDictionary(x => x.ObjectId);
        sql = "select i.* from sys.indexes i join sys.tables t on t.object_id = i.object_id";
        var indexes = connection.Query<Index>(sql).ToList();
        indexes.ForEach(x => tablesDictionary[x.ObjectId].Indexes.Add(x));

        var schema = new Schema(tables);
        var json = JsonConvert.SerializeObject(schema, Formatting.Indented);
        Console.WriteLine(json);

        if (verbose)
        {
            var statistics = new Statistics(schema.SchemaCount(), schema.Tables.Count);
            json = JsonConvert.SerializeObject(statistics, Formatting.Indented);
            Console.Error.WriteLine("Statistics");
            Console.Error.WriteLine(json);
        }
    }
}