using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public class ForeignKeyColumn : Base
{
    [JsonIgnore]
    public int ParentObjectId { get; set; }

    [JsonIgnore]
    public int ConstraintObjectId { get; set; }
}