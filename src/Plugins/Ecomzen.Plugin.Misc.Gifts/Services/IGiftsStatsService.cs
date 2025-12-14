using Ecomzen.Plugin.Misc.Gifts.Domain;
using Nop.Core.Domain.Orders;

namespace Ecomzen.Plugin.Misc.Gifts.Services;

/// <summary>
/// Interface for gifts statistics service
/// </summary>
public interface IGiftsStatsService
{
    /// <summary>
    /// Create gifts statistics for an order
    /// </summary>
    /// <param name="order">Order</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task CreateGiftsStatsAsync(Order order);

    /// <summary>
    /// Get gifts statistics by order ID
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <returns>A task that represents the asynchronous operation with the gifts stats</returns>
    Task<GiftsStats> GetGiftsStatsByOrderIdAsync(int orderId);

    /// <summary>
    /// Update gifts statistics
    /// </summary>
    /// <param name="giftsStats">Gifts statistics</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateGiftsStatsAsync(GiftsStats giftsStats);

    /// <summary>
    /// Delete gifts statistics
    /// </summary>
    /// <param name="giftsStats">Gifts statistics</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteGiftsStatsAsync(GiftsStats giftsStats);
}
