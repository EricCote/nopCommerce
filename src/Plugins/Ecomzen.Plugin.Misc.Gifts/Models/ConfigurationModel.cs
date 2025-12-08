using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Ecomzen.Plugin.Misc.Gifts.Models;

public record ConfigurationModel : BaseNopModel
{
    public ConfigurationModel()
    {
        AvailableCategories = new List<SelectListItem>();
    }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.GiftsCategoryId")]
    public int GiftsCategoryId { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.WalletCategoryId")]
    public int WalletCategoryId { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.PercentageGifts")]
    public decimal PercentageGifts { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.GiftButtonForeground")]
    public string GiftButtonForeground { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.GiftButtonBackground")]
    public string GiftButtonBackground { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.GiftButtonBorder")]
    public string GiftButtonBorder { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.ExceedButtonForeground")]
    public string ExceedButtonForeground { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.ExceedButtonBackground")]
    public string ExceedButtonBackground { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.ExceedButtonBorder")]
    public string ExceedButtonBorder { get; set; }

    public IList<SelectListItem> AvailableCategories { get; set; }
}
