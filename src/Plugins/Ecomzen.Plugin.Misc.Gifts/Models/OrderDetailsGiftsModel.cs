namespace Ecomzen.Plugin.Misc.Gifts.Models;

/// <summary>
/// Model for displaying gift statistics on order details page
/// </summary>
public class OrderDetailsGiftsModel
{
    /// <summary>
    /// Gets or sets the formatted gift value
    /// </summary>
    public string GiftValueFormatted { get; set; }

    /// <summary>
    /// Gets or sets the formatted gift cost
    /// </summary>
    public string GiftCostFormatted { get; set; }

    /// <summary>
    /// Gets or sets the raw gift value
    /// </summary>
    public decimal GiftValue { get; set; }

    /// <summary>
    /// Gets or sets the raw gift cost
    /// </summary>
    public decimal GiftCost { get; set; }
}
