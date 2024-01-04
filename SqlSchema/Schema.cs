namespace SqlSchema;

public class Schema
{
    public Schema(IList<Table> tables)
    {
        Tables = tables;
    }

    public IList<Table> Tables { get; set; }

    public int SchemaCount() => Tables.Select(x => x.Schema).Distinct().Count();
}