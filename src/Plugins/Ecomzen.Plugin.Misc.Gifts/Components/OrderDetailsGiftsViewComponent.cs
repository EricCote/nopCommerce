using Ecomzen.Plugin.Misc.Gifts.Models;
using Ecomzen.Plugin.Misc.Gifts.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;

namespace Ecomzen.Plugin.Misc.Gifts.Components;

/// <summary>
/// View component to display gift statistics on order details page
/// </summary>
public class OrderDetailsGiftsViewComponent : NopViewComponent
{
    private readonly IGiftsStatsService _giftsStatsService;
    private readonly ILocalizationService _localizationService;
    private readonly IPriceFormatter _priceFormatter;
    private readonly IWorkContext _workContext;

    public OrderDetailsGiftsViewComponent(
        IGiftsStatsService giftsStatsService,
        ILocalizationService localizationService,
        IPriceFormatter priceFormatter,
        IWorkContext workContext)
    {
        _giftsStatsService = giftsStatsService;
        _localizationService = localizationService;
        _priceFormatter = priceFormatter;
        _workContext = workContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        // additionalData should be the OrderDetailsModel
        if (additionalData is not Nop.Web.Models.Order.OrderDetailsModel orderDetailsModel)
            return Content("");

        // Get gifts stats for this order
        var giftsStats = await _giftsStatsService.GetGiftsStatsByOrderIdAsync(orderDetailsModel.Id);

        if (giftsStats == null || (giftsStats.GiftValue == 0 && giftsStats.GiftCost == 0))
            return Content("");

        //var language = await _workContext.GetWorkingLanguageAsync();

        var giftValue = await _priceFormatter.FormatPriceAsync(giftsStats.GiftValue, true, false);

        // Create the model
        var model = new OrderDetailsGiftsModel
        {
            GiftValueFormatted = $"({giftValue})",
            GiftCostFormatted = await _priceFormatter.FormatPriceAsync(giftsStats.GiftCost, true, false),
            GiftValue = giftsStats.GiftValue,
            GiftCost = giftsStats.GiftCost
        };

        return View("~/Plugins/Ecomzen.Gifts/Views/OrderDetailsGifts.cshtml", model);
    }
}
