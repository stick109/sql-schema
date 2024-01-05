namespace SqlSchema;

public class Schema
{
    private readonly Dictionary<int, Table> _map;

    public Schema(IList<Table> tables)
    {
        Tables = tables;
        _map = tables.ToDictionary(x => x.ObjectId);
    }

    public IList<Table> Tables { get; set; }

    public void AddColumn(Column column)
    {
        if (_map.TryGetValue(column.ObjectId, out var table))
        {
            table.Columns.Add(column);
        }
    }

    public void AddIndex(Index index)
    {
        if (_map.TryGetValue(index.ObjectId, out var table))
        {
            table.Indexes.Add(index);
        }
    }

    public dynamic Statistics()
    {
        return new
        {
            Schemas = Tables.Select(x => x.Schema).Distinct().Count(),
            Tables = Tables.Count,
            Columns = Tables.Select(x => x.Columns.Count).Sum(),
            Indexes = Tables.Select(x => x.Indexes.Count).Sum(),
        };
    }
}