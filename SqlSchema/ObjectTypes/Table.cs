using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SqlSchema.ObjectTypes;

public record Table : Base
{
    public string Schema { get; set; } = null!;

    public List<Column> Columns { get; set; } = new();
    public List<Index> Indexes { get; set; } = new();

    [JsonIgnore]
    public int ObjectId { get; set; }

    public List<T> GetMember<T>() where T : Base
    {
        switch (typeof(T))
        {
            case { } type when type == typeof(Column):
                return Columns.Select(x => x as T).ToList()!;
            case { } type when type == typeof(Index):
                return Indexes.Select(x => x as T).ToList()!;
            default:
                throw new Exception($"Unknown type {typeof(T).Name}");
        }
    }

    public override string ToString()
    {
        return $"{Schema}.{Name}";
    }

    #region Equality

    public virtual bool Equals(Table? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Name, other.Name, StringComparison) && 
               string.Equals(Schema, other.Schema, StringComparison);
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name, StringComparer);
        hashCode.Add(Schema, StringComparer);
        return hashCode.ToHashCode();
    }

    #endregion
}