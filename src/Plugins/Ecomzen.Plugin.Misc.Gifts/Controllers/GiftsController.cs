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

    #endregion

    #region Ctor

    public GiftsController(
        ISettingService settingService,
        ICategoryService categoryService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _settingService = settingService;
        _categoryService = categoryService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    #endregion

    #region Methods

    public async Task<IActionResult> Configure()
    {
        var settings = await _settingService.LoadSettingAsync<GiftsSettings>();
        
        var model = new ConfigurationModel
        {
            GiftsCategoryId = settings.GiftsCategoryId,
            WalletCategoryId = settings.WalletCategoryId,
            PercentageGifts = settings.PercentageGifts,
            GiftButtonForeground = settings.GiftButtonForeground,
            GiftButtonBackground = settings.GiftButtonBackground,
            GiftButtonBorder = settings.GiftButtonBorder,
            ExceedButtonForeground = settings.ExceedButtonForeground,
            ExceedButtonBackground = settings.ExceedButtonBackground,
            ExceedButtonBorder = settings.ExceedButtonBorder
        };

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
        settings.WalletCategoryId = model.WalletCategoryId;
        settings.PercentageGifts = model.PercentageGifts;
        settings.GiftButtonForeground = model.GiftButtonForeground;
        settings.GiftButtonBackground = model.GiftButtonBackground;
        settings.GiftButtonBorder = model.GiftButtonBorder;
        settings.ExceedButtonForeground = model.ExceedButtonForeground;
        settings.ExceedButtonBackground = model.ExceedButtonBackground;
        settings.ExceedButtonBorder = model.ExceedButtonBorder;
        await _settingService.SaveSettingAsync(settings);

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    #endregion
}

