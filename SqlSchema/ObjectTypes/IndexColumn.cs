using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public class IndexColumn : Base
{
    [JsonProperty(Order = 1)]
    public int IndexColumnId { get; set; }

    [JsonProperty(Order = 2)]
    public bool IsDescendingKey { get; set; }

    [JsonProperty(Order = 3)]
    public bool IsIncludedColumn { get; set; }

    [JsonIgnore]
    public int ObjectId { get; set; }

    [JsonIgnore]
    public int IndexId { get; set; }

    [JsonIgnore]
    public int ColumnId { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IndexColumn) obj);
    }

    protected bool Equals(IndexColumn other)
    {
        return base.Equals(other) 
               && IndexColumnId == other.IndexColumnId 
               && IsDescendingKey == other.IsDescendingKey 
               && IsIncludedColumn == other.IsIncludedColumn;
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), IndexColumnId, IsDescendingKey, IsIncludedColumn);
    }
}