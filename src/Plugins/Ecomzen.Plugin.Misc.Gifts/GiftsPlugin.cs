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

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(GiftsViewComponent);
    }

    public bool HideInWidgetList => false;

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            PublicWidgetZones.OrderSummaryContentAfter // Widget will be displayed in the order summary content area
            // You can change this to any widget zone
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
        // Add localization resources
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Ecomzen.Gifts.Name"] = "Greeting Name",
            ["Plugins.Ecomzen.Gifts.Fields.Name.Hint"] = "Enter the name to display in the greeting alert"
        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
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