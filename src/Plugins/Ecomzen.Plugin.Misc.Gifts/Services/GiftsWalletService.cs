using Ecomzen.Plugin.Misc.Gifts.Models;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Orders;

namespace Ecomzen.Plugin.Misc.Gifts.Services;

/// <summary>
/// Represents the gifts wallet service
/// </summary>
public class GiftsWalletService : IGiftsWalletService
{
    #region Fields

    private readonly ISettingService _settingService;
    private readonly IProductService _productService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly ICategoryService _categoryService;

    #endregion

    #region Ctor

    public GiftsWalletService(
        ISettingService settingService,
        IProductService productService,
        IShoppingCartService shoppingCartService,
        ICategoryService categoryService)
    {
        _settingService = settingService;
        _productService = productService;
        _shoppingCartService = shoppingCartService;
        _categoryService = categoryService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Calculate gift wallet information for a customer
    /// </summary>
    /// <param name="customer">Customer</param>
    /// <param name="storeId">Store identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the gift wallet calculation result
    /// </returns>
    public async Task<GiftsWalletCalculationResult> CalculateGiftWalletAsync(Customer customer, int storeId, bool canDelete)
    {
        var settings = await _settingService.LoadSettingAsync<GiftsSettings>();
        
        var result = new GiftsWalletCalculationResult
        {
            GiftWalletAmount = 0,
            GiftWalletSpent = 0,
            GiftValue =0,
            GiftProductIds = new List<int>(),
            ProductIdsExceedingWallet = new List<int>()
        };

        // Get gift category ID
        var giftsCategoryId = settings.GiftsCategoryId;
        
        if (giftsCategoryId == 0)
            return result;

        // Get shopping cart
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart, storeId);

        // Calculate gift wallet amount based on regular-priced items only
        if (cart.Any() && settings.PercentageGifts > 0)
        {
            decimal regularPricedItemsTotal = 0;

            foreach (var cartItem in cart)
            {
                var product = await _productService.GetProductByIdAsync(cartItem.ProductId);
                
                if (product == null)
                    continue;

                // Get the unit price and discount for this cart item
                var (unitPrice, discountAmount, _) = await _shoppingCartService.GetUnitPriceAsync(cartItem, true);
                
                // Check if product is in wallet category (only if wallet category is configured)
                bool isWalletProduct = false;
                if (settings.WalletCategoryId > 0 && product.Price > 0)
                {
                    var productCategories = await _categoryService.GetProductCategoriesByProductIdAsync(product.Id);
                    isWalletProduct = productCategories.Any(pc => pc.CategoryId == settings.WalletCategoryId);
                }
                
                // Only include items that are:
                // 1. Not discounted (discountAmount == 0)
                // 2. Not in the wallet category
                bool isDiscounted = discountAmount > 0;
 
                
                // If item is regular-priced (not discounted, not on sale, and not a wallet product)
                if (!isDiscounted && !isWalletProduct)
                {
                    // Add the subtotal for this item (unit price * quantity)
                    regularPricedItemsTotal += unitPrice * cartItem.Quantity;
                }

                // Calculate spent amount from gift products (products with price = 0)
                if (product.Price == 0)
                {
                    result.GiftWalletSpent += product.ProductCost * cartItem.Quantity;
                    result.GiftValue += product.OldPrice * cartItem.Quantity;
                }
            }
            
            // Calculate gift wallet amount as percentage of regular-priced items total
            result.GiftWalletAmount = regularPricedItemsTotal * settings.PercentageGifts / 100m;
        }

        if (result.GiftWalletAmount < result.GiftWalletSpent && canDelete){
            // Notify that items are removed
            result.ItemsAreRemoved = true;
            //reset by removing all gifts
            foreach (var cartItem in cart)
            {
                var product = await _productService.GetProductByIdAsync(cartItem.ProductId);
                if (product != null && product.Price == 0)
                {
                    await _shoppingCartService.DeleteShoppingCartItemAsync(cartItem);
                }
            }

        }
      


        // Get ALL gift product IDs from category
        var giftProducts = await _productService.SearchProductsAsync(
            categoryIds: new List<int> { giftsCategoryId },
            storeId: storeId,
            visibleIndividuallyOnly: true,
            pageSize: int.MaxValue // Get all gift products
        );

        result.GiftProductIds = giftProducts.Select(p => p.Id).ToList();

        // Determine which products exceed the available wallet balance
        var products = await _productService.GetProductsByIdsAsync(result.GiftProductIds.ToArray());
        result.ProductIdsExceedingWallet = products
            .Where(p => p.ProductCost > result.RemainingBalance)
            .Select(p => p.Id)
            .ToList();

        return result;
    }

    #endregion
}
