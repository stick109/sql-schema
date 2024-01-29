using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public class Base
{
    protected static StringComparison StringComparison = StringComparison.OrdinalIgnoreCase;
    protected static StringComparer StringComparer = StringComparer.OrdinalIgnoreCase;

    [JsonProperty(Order = 0)]
    public string Name { get; set; } = null!;

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name, StringComparer);
        return hashCode.ToHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Base other) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Name, other.Name, StringComparison);
    }
}