namespace SqlSchema;

public record TableColumnDifference
{
    public string Table { get; set; } = null!;
    public IList<string> MissingInSource { get; set; } = null!;
    public IList<string> MissingInTarget { get; set; } = null!;
    public List<ColumnDifference> Different { get; set;  } = null!;
}