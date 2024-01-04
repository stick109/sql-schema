using Newtonsoft.Json;

namespace SqlSchema;

public record Index
{
    public string Name { get; set; } = null!;
    public bool IsUnique { get; set; }

    [JsonIgnore]
    public int ObjectId { get; set; }
}