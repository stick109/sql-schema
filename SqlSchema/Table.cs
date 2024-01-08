using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SqlSchema;

public record Table
{
    public string Name { get; set; } = null!;
    public string Schema { get; set; } = null!;

    [JsonIgnore]
    public int ObjectId { get; set; }

    public IList<Column> Columns { get; set; } = new List<Column>();
    public IList<Index> Indexes { get; set; } = new List<Index>();

    public virtual bool Equals(Table? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && 
               string.Equals(Schema, other.Schema, StringComparison.InvariantCultureIgnoreCase);
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name, StringComparer.InvariantCultureIgnoreCase);
        hashCode.Add(Schema, StringComparer.InvariantCultureIgnoreCase);
        return hashCode.ToHashCode();
    }
}