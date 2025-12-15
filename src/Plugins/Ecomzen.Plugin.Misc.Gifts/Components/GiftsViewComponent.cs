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
using Nop.Services.Localization;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Ecomzen.Plugin.Misc.Gifts.Components;

public class GiftsViewComponent : NopViewComponent
{
    private readonly ISettingService _settingService;
    private readonly IProductService _productService;

    private readonly IProductModelFactory _productModelFactory;
    private readonly IWorkContext _workContext;
    private readonly IPriceFormatter _priceFormatter;
    private readonly IGiftsWalletService _giftsWalletService;
    private readonly ILocalizationService _localizationService;

    public GiftsViewComponent(
        ISettingService settingService,
        IProductService productService,

        IProductModelFactory productModelFactory,
        IWorkContext workContext,
        IPriceFormatter priceFormatter,
        IGiftsWalletService giftsWalletService,
        ILocalizationService localizationService)
    {
        _settingService = settingService;
        _productService = productService;

        _productModelFactory = productModelFactory;
        _workContext = workContext;
        _priceFormatter = priceFormatter;
        _giftsWalletService = giftsWalletService;
        _localizationService = localizationService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        // Check if we're on a checkout page (not the cart page)
        var routeData = ViewContext.RouteData;
        var controllerName = routeData.Values["controller"]?.ToString();
        var actionName = routeData.Values["action"]?.ToString();
        
        // Don't display on checkout pages, only on cart
        if (!string.Equals(controllerName, "ShoppingCart", StringComparison.InvariantCultureIgnoreCase))
        {
            return Content(string.Empty);
        }

        var settings = await _settingService.LoadSettingAsync<GiftsSettings>();
        
        if (settings.GiftsCategoryId == 0)
            return Content(string.Empty);

 
        var customer = await _workContext.GetCurrentCustomerAsync();
        var workingLanguage = await _workContext.GetWorkingLanguageAsync();

        // Calculate gift wallet using the service
        var walletCalculation = await _giftsWalletService.CalculateGiftWalletAsync(customer,false);

        // Get gift products (limit to 10 for display)
        var products = await _productService.SearchProductsAsync(
            categoryIds: new List<int> { settings.GiftsCategoryId },
            visibleIndividuallyOnly: true,
            pageSize: 10
        );

        // Prepare product overview models using the factory
        var productOverviewModels = await _productModelFactory.PrepareProductOverviewModelsAsync(products);

        var remain = walletCalculation.GiftWalletAmount - walletCalculation.GiftWalletSpent;

        // Get localized description from settings
        var giftsDescription = await _localizationService.GetLocalizedSettingAsync(
            settings, 
            x => x.DescriptionDetails, 
            workingLanguage.Id, 
            0);

        // Get localized title description from settings
        var titleDescription = await _localizationService.GetLocalizedSettingAsync(
            settings, 
            x => x.DescriptionTitle, 
            workingLanguage.Id, 
            0);

        // Get localized locked text from settings
        var lockedText = await _localizationService.GetLocalizedSettingAsync(
            settings, 
            x => x.LockedText, 
            workingLanguage.Id, 
            0);

        // If no custom locked text is set, use the default localization resource
        if (string.IsNullOrEmpty(lockedText))
        {
            lockedText = await _localizationService.GetResourceAsync("Plugins.Ecomzen.Gifts.Locked");
        }

        // Create model
        var model = new GiftsWidgetModel
        {
            GiftWalletAmount = walletCalculation.GiftWalletAmount,
            GiftWalletSpent = walletCalculation.GiftWalletSpent,
            FormattedGiftWalletAmount = await _priceFormatter.FormatPriceAsync(walletCalculation.GiftWalletAmount, true, false),
            FormattedGiftWalletSpent = await _priceFormatter.FormatPriceAsync(walletCalculation.GiftWalletSpent, true, false),
            FormattedGiftWalletRemaining = await _priceFormatter.FormatPriceAsync(remain, true, false),

            DisplayWalletDetails = settings.DisplayWalletDetails,
            GiftsDescription = giftsDescription,
            TitleDescription = titleDescription,
            LockedText = lockedText,
            DescriptionTitleColor = settings.DescriptionTitleColor ?? "#333333",
            DescriptionBackColor = settings.DescriptionBackColor ?? "#F8F9FA",
            DescriptionBorderColor = settings.DescriptionBorderColor ?? "#DEE2E6",
            DescriptionHighlightColor = settings.DescriptionHighlightColor ?? "#3b82f6",
            Products = productOverviewModels
        };
        
        return View("~/Plugins/Ecomzen.Gifts/Views/GiftsWidget.cshtml", model);
    }
}