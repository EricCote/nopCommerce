using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Ecomzen.Plugin.Misc.Gifts.Models;

public record ConfigurationLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.GiftsDescription")]
    public string GiftsDescription { get; set; }

    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.LockedText")]
    public string LockedText { get; set; }
}
