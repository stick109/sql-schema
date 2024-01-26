using System.Diagnostics.CodeAnalysis;

namespace SqlSchema.ObjectTypes;

public record Base
{
    protected static StringComparison StringComparison = StringComparison.OrdinalIgnoreCase;
    protected static StringComparer StringComparer = StringComparer.OrdinalIgnoreCase;

    public string Name { get; set; } = null!;

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name, StringComparer);
        return hashCode.ToHashCode();
    }

    public virtual bool Equals(Base? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Name, other.Name, StringComparison);
    }
}