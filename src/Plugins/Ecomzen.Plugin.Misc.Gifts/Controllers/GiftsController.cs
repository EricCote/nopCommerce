using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Ecomzen.Plugin.Misc.Gifts.Models;
using Ecomzen.Plugin.Misc.Gifts.Services;
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

    private readonly IGiftsGreetingService _greetingService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    #endregion

    #region Ctor

    public GiftsController(
        IGiftsGreetingService greetingService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _greetingService = greetingService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    #endregion

    #region Methods

    public async Task<IActionResult> Configure()
    {
        var greeting = await _greetingService.GetGreetingAsync();
        
        var model = new ConfigurationModel
        {
            Name = greeting?.Name ?? "World"
        };

        return View("~/Plugins/Ecomzen.Gifts/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        var greeting = await _greetingService.GetGreetingAsync();
        
        if (greeting != null)
        {
            greeting.Name = model.Name;
            await _greetingService.UpdateGreetingAsync(greeting);
        }

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    #endregion
}

