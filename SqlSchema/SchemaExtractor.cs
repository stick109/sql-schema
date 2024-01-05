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

        // extract tables
        var sql = "select s.name as [schema], t.* from sys.tables t join sys.schemas s on s.schema_id = t.schema_id";
        var tables = connection.Query<Table>(sql).ToList();
        var schema = new Schema(tables);

        // extract columns
        sql = "select t.name as type, c.* from sys.columns c join sys.types t on t.user_type_id = c.user_type_id";
        var columns = connection.Query<Column>(sql).ToList();
        columns.ForEach(schema.AddColumn);

        // extract indexes
        sql = "select * from sys.indexes";
        var indexes = connection.Query<Index>(sql).ToList();
        indexes.ForEach(schema.AddIndex);

        var json = JsonConvert.SerializeObject(schema, Formatting.Indented);
        Console.WriteLine(json);

        if (verbose)
        {
            json = JsonConvert.SerializeObject(schema.Statistics(), Formatting.Indented);
            Console.Error.WriteLine("Statistics");
            Console.Error.WriteLine(json);
        }
    }
}