using Ecomzen.Plugin.Misc.Gifts.Domain;
using Nop.Data.Mapping;

namespace Ecomzen.Plugin.Misc.Gifts.Data;

/// <summary>
/// Represents backward compatibility of table naming for the Gifts plugin
/// </summary>
public partial class GiftsNameCompatibility : INameCompatibility
{
    /// <summary>
    /// Gets a table names for mapping with entity types
    /// </summary>
    public Dictionary<Type, string> TableNames => new()
    {
        { typeof(GiftsStats), "Ecomzen_Gifts" }
    }
;

    /// <summary>
    /// Gets a column names for mapping with entity property names
    /// </summary>
    public Dictionary<(Type, string), string> ColumnName => new();
}
