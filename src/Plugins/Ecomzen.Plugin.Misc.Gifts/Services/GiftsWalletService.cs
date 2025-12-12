using Ecomzen.Plugin.Misc.Gifts.Models;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
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
    private readonly ICustomerService _customerService;
    private readonly IDiscountService _discountService;

    #endregion

    #region Ctor

    public GiftsWalletService(
        ISettingService settingService,
        IProductService productService,
        IShoppingCartService shoppingCartService,
        ICategoryService categoryService,
        ICustomerService customerService,
        IDiscountService discountService)
    {
        _settingService = settingService;
        _productService = productService;
        _shoppingCartService = shoppingCartService;
        _categoryService = categoryService;
        _customerService = customerService;
        _discountService = discountService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Calculate gift wallet information for a customer
    /// </summary>
    /// <param name="customer">Customer</param>

    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the gift wallet calculation result
    /// </returns>
    public async Task<GiftsWalletCalculationResult> CalculateGiftWalletAsync(Customer customer, bool canDelete)
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
        var excludedCategoryId = settings.ExcludeCategoryId;

        // Get shopping cart
        var cart = await _shoppingCartService.GetShoppingCartAsync(customer, ShoppingCartType.ShoppingCart);

        // Check if any GLOBAL/ORDER-LEVEL discount codes are applied
        var appliedDiscountCodes = await _customerService.ParseAppliedDiscountCouponCodesAsync(customer);
        var hasGlobalDiscountApplied = false;
        
        if (appliedDiscountCodes.Length > 0)
        {
            // Get the actual discount objects for the applied codes
            foreach (var code in appliedDiscountCodes)
            {
                var discounts = await _discountService.GetAllDiscountsAsync(couponCode: code);
                
                // Check if any of these discounts are order-level (global) discounts
                if (discounts.Any(d => 
                    d.DiscountType == DiscountType.AssignedToOrderTotal || 
                    d.DiscountType == DiscountType.AssignedToOrderSubTotal))
                {
                    hasGlobalDiscountApplied = true;
                    break;
                }
            }
        }

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
                bool isExcludedProduct = false;
                if (excludedCategoryId > 0 && product.Price > 0)
                {
                    var productCategories = await _categoryService.GetProductCategoriesByProductIdAsync(product.Id, showHidden: true);
                    isExcludedProduct = productCategories.Any(pc => pc.CategoryId == excludedCategoryId);
                }
                
                // Only include items that are:
                // 1. Not discounted (discountAmount == 0)
                // 2. Not in the wallet category
                // 3. No global discount is applied (optional - you can use hasGlobalDiscountApplied here)
                bool isDiscounted = discountAmount > 0;
 
                
                // If item is regular-priced (not discounted and not a excluded product)
                if (!isDiscounted && !isExcludedProduct)
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
            if (hasGlobalDiscountApplied)
            {
                // If a global discount is applied, set gift wallet amount to zero
                result.GiftWalletAmount = 0;
                result.HasGlobalDiscountApplied = true;
            }
            else
            {
                result.GiftWalletAmount = regularPricedItemsTotal * settings.PercentageGifts / 100m;
                result.HasGlobalDiscountApplied = false;
            }
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
