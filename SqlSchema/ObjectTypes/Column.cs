using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public record Column
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int MaxLength { get; set; }
    public int Precision { get; set; }
    public int Scale { get; set; }

    [JsonIgnore]
    public int ObjectId { get; set; }
}