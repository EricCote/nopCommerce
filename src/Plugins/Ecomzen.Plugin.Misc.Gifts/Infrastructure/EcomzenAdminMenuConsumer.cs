using Nop.Core.Events;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Ecomzen.Plugin.Misc.Gifts.Infrastructure;

/// <summary>
/// Event consumer to add Ecomzen menu to admin sidebar
/// </summary>
public class EcomzenAdminMenuConsumer : IConsumer<AdminMenuCreatedEvent>
{
    /// <summary>
    /// Handle the admin menu created event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        var rootMenuItem = eventMessage.RootMenuItem;

        // Create the top-level Ecomzen menu item
        var ecomzenMenuItem = new AdminMenuItem
        {
            SystemName = "Ecomzen",
            Title = "Ecomzen",
            IconClass = "fas fa-gift",
            Visible = true,
            ChildNodes = new List<AdminMenuItem>
            {
                new AdminMenuItem
                {
                    SystemName = "Ecomzen.Gifts",
                    Title = "Gift Management",
                    Url = "/Admin/Gifts/Configure",
                    IconClass = "far fa-dot-circle",
                    Visible = true
                }
            }
        };

        // Add the menu item to the root
        rootMenuItem.ChildNodes.Add(ecomzenMenuItem);

        return Task.CompletedTask;
    }
}
