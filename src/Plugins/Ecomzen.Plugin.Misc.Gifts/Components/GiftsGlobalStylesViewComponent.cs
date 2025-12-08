// Plugins\Ecomzen.Plugin.Misc.Gifts\Components\GiftsGlobalStylesViewComponent.cs
using Ecomzen.Plugin.Misc.Gifts;
using Ecomzen.Plugin.Misc.Gifts.Models;
using Ecomzen.Plugin.Misc.Gifts.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

public class GiftsGlobalStylesViewComponent : NopViewComponent
{
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;
    private readonly IPriceFormatter _priceFormatter;
    private readonly IGiftsWalletService _giftsWalletService;

    public GiftsGlobalStylesViewComponent(
        ISettingService settingService,
        IStoreContext storeContext,
        IWorkContext workContext, IPriceFormatter priceFormatter,
        IGiftsWalletService giftsWalletService)
    {
        _settingService = settingService;
        _storeContext = storeContext;
        _workContext = workContext;
        _giftsWalletService = giftsWalletService;
        _priceFormatter = priceFormatter;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var settings = await _settingService.LoadSettingAsync<GiftsSettings>();

        if (settings.GiftsCategoryId == 0)
            return Content(string.Empty);

        var store = await _storeContext.GetCurrentStoreAsync();
        var customer = await _workContext.GetCurrentCustomerAsync();

        // Calculate gift wallet using the service
        var walletCalculation = await _giftsWalletService.CalculateGiftWalletAsync(customer, store.Id,true);

        var model = new GiftsGlobalStylesModel
        {
            GiftProductIds = walletCalculation.GiftProductIds,
            ProductIdsExceedingWallet = walletCalculation.ProductIdsExceedingWallet,
            GiftValue = await _priceFormatter.FormatPriceAsync(-walletCalculation.GiftValue, true, false),
            ItemsAreRemoved = walletCalculation.ItemsAreRemoved,
            GiftButtonForeground = settings.GiftButtonForeground,
            GiftButtonBackground = settings.GiftButtonBackground,
            GiftButtonBorder = settings.GiftButtonBorder,
            ExceedButtonForeground = settings.ExceedButtonForeground,
            ExceedButtonBackground = settings.ExceedButtonBackground,
            ExceedButtonBorder = settings.ExceedButtonBorder
        };

        return View("~/Plugins/Ecomzen.Gifts/Views/GiftsGlobalStyles.cshtml", model);
    }
}