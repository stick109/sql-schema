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

        // extract index columns
        sql = @"
select c.name, ic.* from sys.index_columns ic 
join sys.tables t   on t.object_id = ic.object_id
join sys.indexes i  on i.object_id = ic.object_id and i.index_id = ic.index_id
join sys.columns c  on c.object_id = ic.object_id and c.column_id = ic.column_id;
";
        var indexColumns = connection.Query<IndexColumn>(sql).ToList();
        schema.Add(indexColumns);

        // extract foreign keys
        sql = "select * from sys.foreign_keys";
        var foreignKeys = connection.Query<ForeignKey>(sql).ToList();
        schema.Add(foreignKeys);

        // extract foreign keys columns
        sql = @"
select c.name, fkc.* from sys.foreign_key_columns fkc
join sys.columns c on c.object_id = fkc.parent_object_id 
and c.column_id = fkc.parent_column_id;
";
        var foreignKeysColumns = connection.Query<ForeignKeyColumn>(sql).ToList();
        schema.Add(foreignKeysColumns);

        // output result

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