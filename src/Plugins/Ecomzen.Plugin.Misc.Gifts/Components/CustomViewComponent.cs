using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Ecomzen.Plugin.Misc.Gifts.Components;

[ViewComponent(Name = "Custom")]
public class CustomViewComponent : NopViewComponent
{
    public CustomViewComponent()
    {

    }

    public IViewComponentResult Invoke(int productId)
    {
        throw new NotImplementedException();
    }
}
