using Nop.Core;

namespace Ecomzen.Plugin.Misc.Gifts.Domain;

/// <summary>
/// Represents gifts statistics for an order
/// </summary>
public class GiftsStats : BaseEntity
{
    /// <summary>
    /// Gets or sets the order identifier (primary key and foreign key)
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the gift value
    /// </summary>
    public decimal GiftValue { get; set; }

    /// <summary>
    /// Gets or sets the gift cost
    /// </summary>
    public decimal GiftCost { get; set; }
}
