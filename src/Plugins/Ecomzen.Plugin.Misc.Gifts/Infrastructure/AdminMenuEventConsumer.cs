using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;
using System.Threading.Tasks;

namespace Ecomzen.Plugin.Misc.Gifts.Infrastructure;

/// <summary>
/// Event consumer for admin menu
/// </summary>
public class AdminMenuEventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    /// <summary>
    /// Handle the event to add Gifts menu item
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        // Find the Configuration menu item
        var configurationMenuItem = eventMessage.RootMenuItem.GetItemBySystem("Configuration");
        
        if (configurationMenuItem == null)
            return Task.CompletedTask;

        // Create Gifts menu item  
        var giftsMenuItem = new AdminMenuItem
        {
            SystemName = "Gifts",
            Title = "Gifts Configuration",
            IconClass = "far fa-dot-circle",
            Visible = true,
            Url = eventMessage.GetMenuItemUrl("Gifts", "Configure")
        };

        // Add it to the Configuration menu
        configurationMenuItem.ChildNodes.Add(giftsMenuItem);

        return Task.CompletedTask;
    }
}
