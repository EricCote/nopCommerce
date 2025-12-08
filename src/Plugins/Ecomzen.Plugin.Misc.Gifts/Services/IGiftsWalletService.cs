using Ecomzen.Plugin.Misc.Gifts.Models;
using Nop.Core.Domain.Customers;

namespace Ecomzen.Plugin.Misc.Gifts.Services;

/// <summary>
/// Represents the gifts wallet service interface
/// </summary>
public interface IGiftsWalletService
{
    /// <summary>
    /// Calculate gift wallet information for a customer
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <param name="storeId">Store identifier</param>
    /// <param name="canDelete">Can the widgetZone delete gifts that are over allocatio?</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the gift wallet calculation result
    /// </returns>
    Task<GiftsWalletCalculationResult> CalculateGiftWalletAsync(Customer customer, int storeId, bool canDelete);
}
