using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public class Column : Base
{
    [JsonProperty(Order = 1)]
    public string Type { get; set; } = null!;

    [JsonProperty(Order = 2)]
    public int MaxLength { get; set; }

    [JsonProperty(Order = 3)]
    public int Precision { get; set; }

    [JsonProperty(Order = 4)]
    public int Scale { get; set; }

    [JsonIgnore]
    public int ObjectId { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Column other) return false;
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