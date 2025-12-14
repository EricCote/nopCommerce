using Ecomzen.Plugin.Misc.Gifts.Domain;
using Ecomzen.Plugin.Misc.Gifts.Services;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Orders;

namespace Ecomzen.Plugin.Misc.Gifts.Services;

/// <summary>
/// Service for managing gifts statistics
/// </summary>
public class GiftsStatsService : IGiftsStatsService
{
    #region Fields

    private readonly IRepository<GiftsStats> _giftsStatsRepository;
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly GiftsSettings _giftsSettings;

    #endregion

    #region Ctor

    public GiftsStatsService(
        IRepository<GiftsStats> giftsStatsRepository,
        IOrderService orderService,
        IProductService productService,
        ICategoryService categoryService,
        GiftsSettings giftsSettings)
    {
        _giftsStatsRepository = giftsStatsRepository;
        _orderService = orderService;
        _productService = productService;
        _categoryService = categoryService;
        _giftsSettings = giftsSettings;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Create gifts statistics for an order
    /// </summary>
    /// <param name="order">Order</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task CreateGiftsStatsAsync(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        // Check if stats already exist for this order
        var existingStats = await GetGiftsStatsByOrderIdAsync(order.Id);
        if (existingStats != null)
            return;

        // Get all order items
        var orderItems = await _orderService.GetOrderItemsAsync(order.Id);
        
        decimal giftValue = 0;
        decimal giftCost = 0;

        // Calculate gift value and cost from gift products
        foreach (var orderItem in orderItems)
        {
            var product = await _productService.GetProductByIdAsync(orderItem.ProductId);
            if (product == null)
                continue;

            // Check if this product is in the gifts category
            var productCategories = await _categoryService.GetProductCategoriesByProductIdAsync(product.Id);
            var isGiftProduct = productCategories.Any(pc => pc.CategoryId == _giftsSettings.GiftsCategoryId);

            if (isGiftProduct)
            {
                // Sum up OldPrice (gift value) and ProductCost (gift cost)
                giftValue += product.OldPrice * orderItem.Quantity;
                giftCost += orderItem.OriginalProductCost * orderItem.Quantity;
            }
        }

        // Only create stats if there are gift products in the order
        if (giftValue > 0 || giftCost > 0)
        {
            var giftsStats = new GiftsStats
            {
                OrderId = order.Id,
                GiftValue = giftValue,
                GiftCost = giftCost
            };

            await _giftsStatsRepository.InsertAsync(giftsStats);
        }
    }

    /// <summary>
    /// Get gifts statistics by order ID
    /// </summary>
    /// <param name="orderId">Order identifier</param>
    /// <returns>A task that represents the asynchronous operation with the gifts stats</returns>
    public virtual async Task<GiftsStats> GetGiftsStatsByOrderIdAsync(int orderId)
    {
        if (orderId == 0)
            return null;

        return await _giftsStatsRepository.Table
            .FirstOrDefaultAsync(gs => gs.OrderId == orderId);
    }

    /// <summary>
    /// Update gifts statistics
    /// </summary>
    /// <param name="giftsStats">Gifts statistics</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateGiftsStatsAsync(GiftsStats giftsStats)
    {
        if (giftsStats == null)
            throw new ArgumentNullException(nameof(giftsStats));

        await _giftsStatsRepository.UpdateAsync(giftsStats);
    }

    /// <summary>
    /// Delete gifts statistics
    /// </summary>
    /// <param name="giftsStats">Gifts statistics</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteGiftsStatsAsync(GiftsStats giftsStats)
    {
        if (giftsStats == null)
            throw new ArgumentNullException(nameof(giftsStats));

        await _giftsStatsRepository.DeleteAsync(giftsStats);
    }

    #endregion
}
