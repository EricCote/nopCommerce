using Nop.Core.Domain.Orders;
using Nop.Core.Events;
using Nop.Services.Events;
using Ecomzen.Plugin.Misc.Gifts.Services;

namespace Ecomzen.Plugin.Misc.Gifts.Infrastructure;

/// <summary>
/// Event consumer for orders
/// </summary>
public class GiftsOrderEventConsumer : IConsumer<OrderPlacedEvent>
{
    #region Fields

    private readonly IGiftsStatsService _giftsStatsService;

    #endregion

    #region Ctor

    public GiftsOrderEventConsumer(IGiftsStatsService giftsStatsService)
    {
        _giftsStatsService = giftsStatsService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handle order placed event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(OrderPlacedEvent eventMessage)
    {
        if (eventMessage?.Order == null)
            return;

        // Create gifts statistics for the order
        await _giftsStatsService.CreateGiftsStatsAsync(eventMessage.Order);
    }

    #endregion
}
