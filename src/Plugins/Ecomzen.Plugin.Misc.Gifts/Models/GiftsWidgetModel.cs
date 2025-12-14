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

    public decimal giftWalletRemaining => GiftWalletAmount - GiftWalletSpent;
    public string FormattedGiftWalletAmount { get; set; }
    public string FormattedGiftWalletSpent { get; set; }

    public string FormattedGiftWalletRemaining { get; set; }

    public bool DisplayWalletDetails { get; set; }
    public string GiftsDescription { get; set; }
    public string TitleDescription { get; set; }
    public string LockedText { get; set; }
    public IEnumerable<ProductOverviewModel> Products { get; set; }
}
