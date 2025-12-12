using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Ecomzen.Plugin.Misc.Gifts.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Mvc.Filters;

namespace Ecomzen.Plugin.Misc.Gifts.Controllers;

[AutoValidateAntiforgeryToken]
[AuthorizeAdmin] //confirms access to the admin panel
[Area(AreaNames.ADMIN)] //specifies the area containing a controller or action
public class GiftsController : BasePluginController
{
    #region Fields

    private readonly ISettingService _settingService;
    private readonly ICategoryService _categoryService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly ILocalizedModelFactory _localizedModelFactory;


    #endregion

    #region Ctor

    public GiftsController(
        ISettingService settingService,
        ICategoryService categoryService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        ILocalizedModelFactory localizedModelFactory
)
    {
        _settingService = settingService;
        _categoryService = categoryService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _localizedModelFactory = localizedModelFactory;

    }

    #endregion

    #region Methods

    public async Task<IActionResult> Configure()
    {

        var settings = await _settingService.LoadSettingAsync<GiftsSettings>();
        
        var model = new ConfigurationModel
        {
            GiftsCategoryId = settings.GiftsCategoryId,
            ExcludeCategoryId = settings.ExcludeCategoryId,
            PercentageGifts = settings.PercentageGifts,
            DisplayWalletDetails = settings.DisplayWalletDetails,
            GiftsDescription = settings.GiftsDescription,
            LockedText = settings.LockedText,
            GiftButtonForeground = settings.GiftButtonForeground,
            GiftButtonBackground = settings.GiftButtonBackground,
            GiftButtonBorder = settings.GiftButtonBorder,
            ExceedButtonForeground = settings.ExceedButtonForeground,
            ExceedButtonBackground = settings.ExceedButtonBackground,
            ExceedButtonBorder = settings.ExceedButtonBorder
        };

        model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync<ConfigurationLocalizedModel>(async (locale, languageId) =>
        {
            locale.GiftsDescription = await _localizationService.GetLocalizedSettingAsync(settings, x => x.GiftsDescription, languageId, 0, false);
            locale.LockedText = await _localizationService.GetLocalizedSettingAsync(settings, x => x.LockedText, languageId, 0, false);
        });

        // Populate categories dropdown
        var categories = await _categoryService.GetAllCategoriesAsync(showHidden: true);
        model.AvailableCategories.Add(new SelectListItem
        {
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select"),
            Value = "0"
        });
        
        foreach (var category in categories)
        {
            model.AvailableCategories.Add(new SelectListItem
            {
                Text = await _categoryService.GetFormattedBreadCrumbAsync(category, categories),
                Value = category.Id.ToString()
            });
        }

        return View("~/Plugins/Ecomzen.Gifts/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        var settings = await _settingService.LoadSettingAsync<GiftsSettings>();
        
        settings.GiftsCategoryId = model.GiftsCategoryId;
        settings.ExcludeCategoryId = model.ExcludeCategoryId;
        settings.PercentageGifts = model.PercentageGifts;
        settings.DisplayWalletDetails = model.DisplayWalletDetails;
        settings.GiftsDescription = model.GiftsDescription ?? "<p>A gift is available when it is green. </p>";
        settings.LockedText = model.LockedText ?? "🔒 Locked";
        settings.GiftButtonForeground = model.GiftButtonForeground ?? "#FFFFFF";
        settings.GiftButtonBackground = model.GiftButtonBackground ?? "#4CAF50";
        settings.GiftButtonBorder = model.GiftButtonBorder ?? "#4CAF50";
        settings.ExceedButtonForeground = model.ExceedButtonForeground ?? "#FFFFFF";
        settings.ExceedButtonBackground = model.ExceedButtonBackground ?? "#DC3545";
        settings.ExceedButtonBorder = model.ExceedButtonBorder ?? "#DC3545";
        
        // Save all settings (this creates the Setting records in the database)
        await _settingService.SaveSettingAsync(settings);


        // Reload settings to ensure we have the freshly saved Setting record IDs
        settings = await _settingService.LoadSettingAsync<GiftsSettings>();

        // Now save localized versions
        foreach (var localized in model.Locales)
        {
            await _localizationService.SaveLocalizedSettingAsync(settings, 
                x => x.GiftsDescription, 
                localized.LanguageId, 
                localized.GiftsDescription ?? string.Empty);
            
            await _localizationService.SaveLocalizedSettingAsync(settings, 
                x => x.LockedText, 
                localized.LanguageId, 
                localized.LockedText ?? string.Empty);
        }

        // Clear cache one final time
        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    #endregion
}

