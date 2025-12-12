using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Web.Models.Catalog;

namespace Ecomzen.Plugin.Misc.Gifts.Models;

public class GiftsGlobalStylesModel
{
    public GiftsGlobalStylesModel()
    {
        GiftProductIds = new List<int>();
        ProductIdsExceedingWallet = new List<int>();
        GiftValue = "0";
        ItemsAreRemoved = false;
        HasGlobalDiscountApplied = false;
    }
    public List<int> ProductIdsExceedingWallet { get; set; }
    public List<int> GiftProductIds { get; set; }

    public string GiftValue { get; set; }
    public bool ItemsAreRemoved { get; set; }
    public bool HasGlobalDiscountApplied { get; set; }

    // Color settings
    public string GiftButtonForeground { get; set; }
    public string GiftButtonBackground { get; set; }
    public string GiftButtonBorder { get; set; }
    public string ExceedButtonForeground { get; set; }
    public string ExceedButtonBackground { get; set; }
    public string ExceedButtonBorder { get; set; }

    // Hover colors (calculated)
    public string GiftButtonBackgroundHover { get; set; }
    public string GiftButtonBorderHover { get; set; }
    public string ExceedButtonBackgroundHover { get; set; }
    public string ExceedButtonBorderHover { get; set; }

    // Localized texts
    public string LockedText { get; set; }

    /// <summary>
    /// Calculate hover color by darkening the original color by a percentage
    /// </summary>
    /// <param name="hexColor">Hex color string (e.g., #4CAF50)</param>
    /// <param name="darkenPercent">Percentage to darken (default 10%)</param>
    /// <returns>Darkened hex color</returns>
    public static string CalculateHoverColor(string hexColor, int darkenPercent = 10)
    {
        if (string.IsNullOrEmpty(hexColor))
            return hexColor;

        // Remove # if present
        hexColor = hexColor.TrimStart('#');

        // Validate hex color
        if (hexColor.Length != 6)
            return $"#{hexColor}";

        try
        {
            // Convert to RGB
            var r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
            var g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
            var b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

            // Darken by percentage
            var factor = 1 - (darkenPercent / 100.0);
            var newR = (int)Math.Round(r * factor);
            var newG = (int)Math.Round(g * factor);
            var newB = (int)Math.Round(b * factor);

            // Ensure values are within valid range
            newR = Math.Max(0, Math.Min(255, newR));
            newG = Math.Max(0, Math.Min(255, newG));
            newB = Math.Max(0, Math.Min(255, newB));

            // Convert back to hex
            return $"#{newR:X2}{newG:X2}{newB:X2}";
        }
        catch
        {
            // Return original color if parsing fails
            return $"#{hexColor}";
        }
    }
}
