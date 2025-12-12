using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Ecomzen.Plugin.Misc.Gifts.Models;

public record ConfigurationModel : BaseNopModel, ILocalizedModel<ConfigurationLocalizedModel>
{
    public ConfigurationModel()
    {
        AvailableCategories = new List<SelectListItem>();
        Locales = new List<ConfigurationLocalizedModel>();
    }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.GiftsCategoryId")]
    public int GiftsCategoryId { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.ExcludeCategoryId")]
    public int ExcludeCategoryId { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.PercentageGifts")]
    public decimal PercentageGifts { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.DisplayWalletDetails")]
    public bool DisplayWalletDetails { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.GiftsDescription")]
    public string GiftsDescription { get; set; }

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

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.LockedText")]
    public string LockedText { get; set; }

    public IList<SelectListItem> AvailableCategories { get; set; }
    
    public IList<ConfigurationLocalizedModel> Locales { get; set; }
}
