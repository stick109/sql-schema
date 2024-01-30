using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public class ForeignKey : Base
{
    [JsonProperty(Order = 1)]
    public List<ForeignKeyColumn> ForeignKeyColumns { get; set; } = new();

    [JsonIgnore]
    public int ObjectId { get; set; }

    [JsonIgnore]
    public int ParentObjectId { get; set; }
}