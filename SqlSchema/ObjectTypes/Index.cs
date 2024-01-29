using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public class Index : Base
{
    [JsonProperty(PropertyName = "Type", Order = 1)]
    public string TypeDesc { get; set; } = null!;

    [JsonProperty(Order = 2)]
    public bool IsUnique { get; set; }

    [JsonProperty(Order = 3)]
    public List<IndexColumn> IndexColumns { get; set; } = new ();

    [JsonIgnore]
    public int ObjectId { get; set; }

    [JsonIgnore]
    public int IndexId { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Index other) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) 
               && string.Equals(TypeDesc, other.TypeDesc, StringComparison)
               && IsUnique == other.IsUnique
               && IndexColumns.OrderBy(x => x.IndexColumnId).SequenceEqual(other
                   .IndexColumns.OrderBy(x => x.IndexColumnId));
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), IsUnique, TypeDesc);
    }
}