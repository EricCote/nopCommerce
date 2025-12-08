using Nop.Web.Models.Catalog;

namespace Ecomzen.Plugin.Misc.Gifts.Models;

public record GiftsWidgetModel
{
    public GiftsWidgetModel()
    {
        Products = new List<ProductOverviewModel>();
    }

    public decimal GiftWalletAmount { get; set; }
    public decimal GiftWalletSpent { get; set; }
    public string FormattedGiftWalletAmount { get; set; }
    public string FormattedGiftWalletSpent { get; set; }
    public IEnumerable<ProductOverviewModel> Products { get; set; }

}
