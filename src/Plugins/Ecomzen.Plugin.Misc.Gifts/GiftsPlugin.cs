using Nop.Core;
using Nop.Core.Domain.Cms;
using Ecomzen.Plugin.Misc.Gifts.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;


namespace Ecomzen.Plugin.Misc.Gifts;

/// <summary>
/// Represents the Gifts plugin
/// </summary>
public class GiftsPlugin : BasePlugin, IWidgetPlugin
{
    #region Fields

    protected readonly IWebHelper _webHelper;
    protected readonly ILocalizationService _localizationService;
    protected readonly ISettingService _settingService;
    protected readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public GiftsPlugin(
        IWebHelper webHelper, 
        ILocalizationService localizationService,
        ISettingService settingService,
        WidgetSettings widgetSettings)
    {
        _webHelper = webHelper;
        _localizationService = localizationService;
        _settingService = settingService;
        _widgetSettings = widgetSettings;
    }

    #endregion

    #region Methods


    public bool HideInWidgetList => false;


    public Type GetWidgetViewComponent(string widgetZone)
    {
        if (widgetZone == PublicWidgetZones.HeadHtmlTag)
            return typeof(GiftsGlobalStylesViewComponent);
        
        if (widgetZone == PublicWidgetZones.OrderSummaryContentAfter)
            return typeof(GiftsViewComponent);
        
        return typeof(GiftsViewComponent);
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            PublicWidgetZones.OrderSummaryContentAfter,
            PublicWidgetZones.HeadHtmlTag             // Global CSS/JS injection
    
        });
    }

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/Gifts/Configure";
    }

    /// <summary>
    /// Install the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        // Install default settings
        await _settingService.SaveSettingAsync(new GiftsSettings
        {
            GiftsCategoryId = 0,
            WalletCategoryId = 0,
            PercentageGifts = 10m,
            GiftButtonForeground = "#FFFFFF",
            GiftButtonBackground = "#4CAF50",
            GiftButtonBorder = "#4CAF50",
            ExceedButtonForeground = "#FFFFFF",
            ExceedButtonBackground = "#DC3545",
            ExceedButtonBorder = "#DC3545"
        });

        // Add localization resources
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Ecomzen.Gifts.Fields.GiftsCategoryId"] = "Gift Category",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsCategoryId.Hint"] = "Select the category for gift products",
            ["Plugins.Ecomzen.Gifts.Fields.WalletCategoryId"] = "Wallet Exclusion Category",
            ["Plugins.Ecomzen.Gifts.Fields.WalletCategoryId.Hint"] = "Select the category to exlude products from the Gift Wallet",
            ["Plugins.Ecomzen.Gifts.Fields.PercentageGifts"] = "Gift Percentage",
            ["Plugins.Ecomzen.Gifts.Fields.PercentageGifts.Hint"] = "Enter the percentage for gifts (e.g., 10 for 10%)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonForeground"] = "Gift Button Text Color",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonForeground.Hint"] = "Color for gift button text (e.g., #FFFFFF)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBackground"] = "Gift Button Background Color",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBackground.Hint"] = "Background color for gift buttons (e.g., #4CAF50)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBorder"] = "Gift Button Border Color",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBorder.Hint"] = "Border color for gift buttons (e.g., #4CAF50)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonForeground"] = "Exceeded Wallet Button Text Color",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonForeground.Hint"] = "Color for exceeded wallet button text (e.g., #FFFFFF)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBackground"] = "Exceeded Wallet Button Background Color",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBackground.Hint"] = "Background color for exceeded wallet buttons (e.g., #DC3545)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBorder"] = "Exceeded Wallet Button Border Color",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBorder.Hint"] = "Border color for exceeded wallet buttons (e.g., #DC3545)",
            ["Plugins.Ecomzen.Gifts.GiftWallet"] = "Your Gift Wallet",
            ["Plugins.Ecomzen.Gifts.NoProducts"] = "No gifts are available at this time.",
            ["Plugins.Ecomzen.Gifts.Spent"] = "Spent",
            ["Plugins.Ecomzen.Gifts.Remaining"] = "Remaining Balance",
            ["Plugins.Ecomzen.Gifts.Locked"] = "Locked",
            ["Plugins.Ecomzen.Gifts.InsufficientWallet"] = "Insufficient gift wallet balance for this product",
            ["Plugins.Ecomzen.Gifts.TotalValue"] = "Total value of your gifts:",
            ["Plugins.Ecomzen.Gifts.ItemsRemovedNotification"] = "Gifts were removed because you need to add more products."
        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        // Delete settings
        await _settingService.DeleteSettingAsync<GiftsSettings>();

        // Deactivate widget if it's active
        if (_widgetSettings.ActiveWidgetSystemNames.Contains("Ecomzen.Gifts"))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove("Ecomzen.Gifts");
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        // Delete localization resources
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Ecomzen.Gifts");

        await base.UninstallAsync();
    }

    #endregion
}