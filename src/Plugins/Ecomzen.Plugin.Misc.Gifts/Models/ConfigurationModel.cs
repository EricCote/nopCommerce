using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Ecomzen.Plugin.Misc.Gifts.Models;

public record ConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Ecomzen.Gifts.Fields.Name")]
    public string Name { get; set; }
}
