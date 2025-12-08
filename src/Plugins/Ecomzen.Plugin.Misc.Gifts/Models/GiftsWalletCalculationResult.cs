namespace Ecomzen.Plugin.Misc.Gifts.Models;

/// <summary>
/// Represents the result of gift wallet calculation
/// </summary>
public class GiftsWalletCalculationResult
{
    /// <summary>
    /// Gets or sets the total gift wallet amount available
    /// </summary>
    public decimal GiftWalletAmount { get; set; }

    /// <summary>
    /// Gets or sets the amount already spent from the gift wallet
    /// </summary>
    public decimal GiftWalletSpent { get; set; }


    /// <summary>
    /// Gets or sets the total gift value
    /// </summary>
    public decimal GiftValue{ get; set; }

    /// <summary>
    /// Gets or sets the list of gift product IDs
    /// </summary>
    public List<int> GiftProductIds { get; set; }

    /// <summary>
    /// Gets or sets the list of product IDs that exceed the available wallet balance
    /// </summary>
    public List<int> ProductIdsExceedingWallet { get; set; }

    /// <summary>
    /// Gets the remaining gift wallet balance
    /// </summary>
    public decimal RemainingBalance => GiftWalletAmount - GiftWalletSpent;

    public bool ItemsAreRemoved { get; set; }
}
