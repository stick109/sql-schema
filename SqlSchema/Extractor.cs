using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SqlSchema.ObjectTypes;
using SqlSchema.Options;
using Index = SqlSchema.ObjectTypes.Index;

namespace SqlSchema;

public class Extractor
{
    public static void Run(Extract options)
    {
        options.Print();

        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = options.Server,
            InitialCatalog = options.Database,
            IntegratedSecurity = true,
            TrustServerCertificate = true,
        };
        if (options.Debug)
        {
            Console.Error.WriteLine($"Connecting as [{builder.ConnectionString}]");
        }

        using var connection = new SqlConnection(builder.ConnectionString);
        var schema = new Schema();

        // extract tables
        var sql = "select s.name as [schema], t.* from sys.tables t join sys.schemas s on s.schema_id = t.schema_id";
        var tables = connection.Query<Table>(sql).ToList();
        schema.Add(tables);

        // extract columns
        sql = "select t.name as type, c.* from sys.columns c join sys.types t on t.user_type_id = c.user_type_id";
        var columns = connection.Query<Column>(sql).ToList();
        schema.Add(columns);

        // extract indexes
        sql = "select * from sys.indexes where type != 0";
        var indexes = connection.Query<Index>(sql).ToList();
        schema.Add(indexes);

        var json = JsonConvert.SerializeObject(schema, Formatting.Indented);
        Console.WriteLine(json);

        if (options.Verbose)
        {
            json = JsonConvert.SerializeObject(schema.Statistics(), Formatting.Indented);
            Console.Error.WriteLine("Statistics:");
            Console.Error.WriteLine(json);
        }
    }
}