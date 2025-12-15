using Nop.Core.Configuration;

namespace Ecomzen.Plugin.Misc.Gifts;

/// <summary>
/// Represents settings for the Gifts plugin
/// </summary>
public class GiftsSettings : ISettings
{
    /// <summary>
    /// Gets or sets the gifts category ID
    /// </summary>
    public int GiftsCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the wallet category ID
    /// </summary>
    public int ExcludeCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the percentage for gifts
    /// </summary>
    public decimal PercentageGifts { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display wallet details
    /// </summary>
    public bool DisplayWalletDetails { get; set; } = true;

    /// <summary>
    /// Gets or sets the gifts description
    /// </summary>
    public string DescriptionDetails { get; set; }

    /// <summary>
    /// Gets or sets the title description
    /// </summary>
    public string DescriptionTitle { get; set; }

    /// <summary>
    /// Gets or sets the gift button foreground color
    /// </summary>
    public string GiftButtonForeground { get; set; } = "#FFFFFF";

    /// <summary>
    /// Gets or sets the gift button background color
    /// </summary>
    public string GiftButtonBackground { get; set; } = "#4CAF50";

    /// <summary>
    /// Gets or sets the gift button border color
    /// </summary>
    public string GiftButtonBorder { get; set; } = "#4CAF50";

    /// <summary>
    /// Gets or sets the exceed button foreground color
    /// </summary>
    public string ExceedButtonForeground { get; set; } = "#FFFFFF";

    /// <summary>
    /// Gets or sets the exceed button background color
    /// </summary>
    public string ExceedButtonBackground { get; set; } = "#DC3545";

    /// <summary>
    /// Gets or sets the exceed button border color
    /// </summary>
    public string ExceedButtonBorder { get; set; } = "#DC3545";

    /// <summary>
    /// Gets or sets the locked gift text
    /// </summary>
    public string LockedText { get; set; }

    /// <summary>
    /// Gets or sets the description title color
    /// </summary>
    public string DescriptionTitleColor { get; set; } = "#333333";

    /// <summary>
    /// Gets or sets the description background color
    /// </summary>
    public string DescriptionBackColor { get; set; } = "#F8F9FA";

    /// <summary>
    /// Gets or sets the description border color
    /// </summary>
    public string DescriptionBorderColor { get; set; } = "#DEE2E6";

    /// <summary>
    /// Gets or sets the description highlight color
    /// </summary>
    public string DescriptionHighlightColor { get; set; } = "#3b82f6";
}
