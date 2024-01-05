using Newtonsoft.Json;

namespace SqlSchema;

public record Table
{
    public string Name { get; set; } = null!;
    public string Schema { get; set; } = null!;

    [JsonIgnore]
    public int ObjectId { get; set; }

    public IList<Column> Columns { get; set; } = new List<Column>();
    public IList<Index> Indexes { get; set; } = new List<Index>();
}