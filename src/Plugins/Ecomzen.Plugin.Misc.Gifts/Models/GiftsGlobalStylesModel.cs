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
    }
    public List<int> ProductIdsExceedingWallet { get; set; }
    public List<int> GiftProductIds { get; set; }

    public string GiftValue { get; set; }
    public bool ItemsAreRemoved { get; set; }

    // Color settings
    public string GiftButtonForeground { get; set; }
    public string GiftButtonBackground { get; set; }
    public string GiftButtonBorder { get; set; }
    public string ExceedButtonForeground { get; set; }
    public string ExceedButtonBackground { get; set; }
    public string ExceedButtonBorder { get; set; }
}
