namespace SqlSchema.ObjectTypes;

public class Schema
{
    private Dictionary<int, Table>? _map;

    public List<Table> Tables { get; } = new();

    private void InitMap()
    {
        _map ??= Tables.ToDictionary(x => x.ObjectId);
    }

    public void Add(List<Table> tables)
    {
        Tables.AddRange(tables);
    }

    public void Add(List<Column> columns)
    {
        InitMap();
        columns.ForEach(column =>
        {
            if (_map!.TryGetValue(column.ObjectId, out var table))
            {
                table.Columns.Add(column);
            }
        });
    }

    public void Add(List<Index> indexes)
    {
        indexes.ForEach(index =>
        {
            if (_map!.TryGetValue(index.ObjectId, out var table))
            {
                table.Indexes.Add(index);
            }
        });
    }

    public void Add(List<IndexColumn> indexColumns)
    {
        indexColumns.ForEach(indexColumn =>
        {
            if (_map!.TryGetValue(indexColumn.ObjectId, out var table))
            {
                var index = table.Indexes.First(x => x.IndexId == indexColumn.IndexId);
                index.IndexColumns.Add(indexColumn);
            }
        });
    }

    public void Add(List<ForeignKey> foreignKeys)
    {
        foreignKeys.ForEach(foreignKey =>
        {
            if (_map!.TryGetValue(foreignKey.ParentObjectId, out var table))
            {
                table.ForeignKeys.Add(foreignKey);
            }
        });
    }

    public void Add(List<ForeignKeyColumn> foreignKeyColumns)
    {
        foreignKeyColumns.ForEach(foreignKeyColumn =>
        {
            if (_map!.TryGetValue(foreignKeyColumn.ParentObjectId, out var table))
            {
                var foreignKey = table.ForeignKeys.First(x => x.ObjectId == foreignKeyColumn.ConstraintObjectId);
                foreignKey.ForeignKeyColumns.Add(foreignKeyColumn);
            }
        });
    }

    public (IList<Table>, IList<Table>, IList<Table>) CompareWith(Schema target)
    {
        return default;
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