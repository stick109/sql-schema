using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public enum IndexType : byte
{
    Clustered = 1,
    NonClustered
}

public record Index : Base
{
    public bool IsUnique { get; set; }
    public IndexType Type { get; set; }

    [JsonIgnore]
    public int ObjectId { get; set; }

    [JsonIgnore]
    public int IndexId { get; set; }

    public virtual bool Equals(Index? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) 
               && IsUnique == other.IsUnique 
               && Type == other.Type;
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), IsUnique, (int) Type);
    }
}