using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public record Column : Base
{
    public string Type { get; set; } = null!;
    public int MaxLength { get; set; }
    public int Precision { get; set; }
    public int Scale { get; set; }

    [JsonIgnore]
    public int ObjectId { get; set; }

    public virtual bool Equals(Column? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other)
               && Type == other.Type 
               && MaxLength == other.MaxLength 
               && Precision == other.Precision 
               && Scale == other.Scale;
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Type, MaxLength, Precision, Scale);
    }
}