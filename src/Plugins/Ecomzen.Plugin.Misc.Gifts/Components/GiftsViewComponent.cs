using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecomzen.Plugin.Misc.Gifts.Services;
using Nop.Web.Framework.Components;

namespace Ecomzen.Plugin.Misc.Gifts.Components;


public class GiftsViewComponent : NopViewComponent
{
    private readonly IGiftsGreetingService _greetingService;

    public GiftsViewComponent(IGiftsGreetingService greetingService)
    {
        _greetingService = greetingService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var greeting = await _greetingService.GetGreetingAsync();
        var name = greeting?.Name ?? "World";
        
        return View("~/Plugins/Ecomzen.Gifts/Views/PublicInfo.cshtml", name);
    }
}