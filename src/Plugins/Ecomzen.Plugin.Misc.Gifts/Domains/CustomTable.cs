using Nop.Core;

namespace Ecomzen.Plugin.Misc.Gifts.Domains;

public partial class AlloGreeting : BaseEntity
{
    /// <summary>
    /// Gets or sets the name to display in the greeting
    /// </summary>
    public string Name { get; set; }
}