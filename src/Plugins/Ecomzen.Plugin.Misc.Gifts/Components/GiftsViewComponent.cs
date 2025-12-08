using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Ecomzen.Plugin.Misc.Gifts.Models;
using Ecomzen.Plugin.Misc.Gifts.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Ecomzen.Plugin.Misc.Gifts.Components;

public class GiftsViewComponent : NopViewComponent
{
    private readonly ISettingService _settingService;
    private readonly IProductService _productService;
    private readonly IStoreContext _storeContext;
    private readonly IProductModelFactory _productModelFactory;
    private readonly IWorkContext _workContext;
    private readonly IPriceFormatter _priceFormatter;
    private readonly IGiftsWalletService _giftsWalletService;

    public GiftsViewComponent(
        ISettingService settingService,
        IProductService productService,
        IStoreContext storeContext,
        IProductModelFactory productModelFactory,
        IWorkContext workContext,
        IPriceFormatter priceFormatter,
        IGiftsWalletService giftsWalletService)
    {
        _settingService = settingService;
        _productService = productService;
        _storeContext = storeContext;
        _productModelFactory = productModelFactory;
        _workContext = workContext;
        _priceFormatter = priceFormatter;
        _giftsWalletService = giftsWalletService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var settings = await _settingService.LoadSettingAsync<GiftsSettings>();
        
        if (settings.GiftsCategoryId == 0)
            return Content(string.Empty);

        var store = await _storeContext.GetCurrentStoreAsync();
        var customer = await _workContext.GetCurrentCustomerAsync();

        // Calculate gift wallet using the service
        var walletCalculation = await _giftsWalletService.CalculateGiftWalletAsync(customer, store.Id,false);

        // Get gift products (limit to 10 for display)
        var products = await _productService.SearchProductsAsync(
            categoryIds: new List<int> { settings.GiftsCategoryId },
            storeId: store.Id,
            visibleIndividuallyOnly: true,
            pageSize: 10
        );

        // Prepare product overview models using the factory
        var productOverviewModels = await _productModelFactory.PrepareProductOverviewModelsAsync(products);
        
        // Create model
        var model = new GiftsWidgetModel
        {
            GiftWalletAmount = walletCalculation.GiftWalletAmount,
            GiftWalletSpent = walletCalculation.GiftWalletSpent,
            FormattedGiftWalletAmount = await _priceFormatter.FormatPriceAsync(walletCalculation.GiftWalletAmount, true, false),
            FormattedGiftWalletSpent = await _priceFormatter.FormatPriceAsync(walletCalculation.GiftWalletSpent, true, false),
            Products = productOverviewModels
        };
        
        return View("~/Plugins/Ecomzen.Gifts/Views/GiftsWidget.cshtml", model);
    }
}