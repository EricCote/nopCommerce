// Plugins\Ecomzen.Plugin.Misc.Gifts\Components\GiftsGlobalStylesViewComponent.cs
using Ecomzen.Plugin.Misc.Gifts;
using Ecomzen.Plugin.Misc.Gifts.Models;
using Ecomzen.Plugin.Misc.Gifts.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;

public class GiftsGlobalStylesViewComponent : NopViewComponent
{
    private readonly ISettingService _settingService;
    private readonly IWorkContext _workContext;
    private readonly IPriceFormatter _priceFormatter;
    private readonly IGiftsWalletService _giftsWalletService;
    private readonly ILocalizationService _localizationService;

    public GiftsGlobalStylesViewComponent(
        ISettingService settingService,

        IWorkContext workContext, 
        IPriceFormatter priceFormatter,
        IGiftsWalletService giftsWalletService,
        ILocalizationService localizationService)
    {
        _settingService = settingService;
   
        _workContext = workContext;
        _giftsWalletService = giftsWalletService;
        _priceFormatter = priceFormatter;
        _localizationService = localizationService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var settings = await _settingService.LoadSettingAsync<GiftsSettings>();

        if (settings.GiftsCategoryId == 0)
            return Content(string.Empty);

        var customer = await _workContext.GetCurrentCustomerAsync();
        var workingLanguage = await _workContext.GetWorkingLanguageAsync();

        // Calculate gift wallet using the service
        var walletCalculation = await _giftsWalletService.CalculateGiftWalletAsync(customer, true);

        // Get localized locked text from settings
        var lockedText = await _localizationService.GetLocalizedSettingAsync(
            settings, 
            x => x.LockedText, 
            workingLanguage.Id, 
            0);


        var giftValue = await _priceFormatter.FormatPriceAsync(walletCalculation.GiftValue, true, false);

        var model = new GiftsGlobalStylesModel
        {
            GiftProductIds = walletCalculation.GiftProductIds,
            ProductIdsExceedingWallet = walletCalculation.ProductIdsExceedingWallet,
            GiftValue = $"({giftValue})",
            ItemsAreRemoved = walletCalculation.ItemsAreRemoved,
            HasGlobalDiscountApplied = walletCalculation.HasGlobalDiscountApplied,
            GiftButtonForeground = settings.GiftButtonForeground,
            GiftButtonBackground = settings.GiftButtonBackground,
            GiftButtonBorder = settings.GiftButtonBorder,
            ExceedButtonForeground = settings.ExceedButtonForeground,
            ExceedButtonBackground = settings.ExceedButtonBackground,
            ExceedButtonBorder = settings.ExceedButtonBorder,
            LockedText = lockedText,
            // Calculate hover colors using the static method
            GiftButtonBackgroundHover = GiftsGlobalStylesModel.CalculateHoverColor(settings.GiftButtonBackground, 10),
            GiftButtonBorderHover = GiftsGlobalStylesModel.CalculateHoverColor(settings.GiftButtonBorder, 10),
            ExceedButtonBackgroundHover = GiftsGlobalStylesModel.CalculateHoverColor(settings.ExceedButtonBackground, 10),
            ExceedButtonBorderHover = GiftsGlobalStylesModel.CalculateHoverColor(settings.ExceedButtonBorder, 10)
        };

        return View("~/Plugins/Ecomzen.Gifts/Views/GiftsGlobalStyles.cshtml", model);
    }
}