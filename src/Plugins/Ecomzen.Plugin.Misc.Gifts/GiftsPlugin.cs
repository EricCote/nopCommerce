using Nop.Core;
using Nop.Core.Domain.Cms;
using Ecomzen.Plugin.Misc.Gifts.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;


namespace Ecomzen.Plugin.Misc.Gifts;

/// <summary>
/// Represents the Gifts plugin
/// </summary>
public class GiftsPlugin : BasePlugin, IWidgetPlugin
{
    #region Fields

    protected readonly IWebHelper _webHelper;
    protected readonly ILocalizationService _localizationService;
    protected readonly ISettingService _settingService;
    protected readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public GiftsPlugin(
        IWebHelper webHelper, 
        ILocalizationService localizationService,
        ISettingService settingService,
        WidgetSettings widgetSettings)
    {
        _webHelper = webHelper;
        _localizationService = localizationService;
        _settingService = settingService;
        _widgetSettings = widgetSettings;
    }

    #endregion

    #region Methods


    public bool HideInWidgetList => false;


    public Type GetWidgetViewComponent(string widgetZone)
    {
        if (widgetZone == PublicWidgetZones.HeadHtmlTag)
            return typeof(GiftsGlobalStylesViewComponent);
        
        if (widgetZone == PublicWidgetZones.OrderSummaryContentAfter)
            return typeof(GiftsViewComponent);
        
        if (widgetZone == PublicWidgetZones.OrderDetailsPageOverview)
            return typeof(OrderDetailsGiftsViewComponent);
        
        return typeof(GiftsViewComponent);
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            PublicWidgetZones.OrderSummaryContentAfter,
            PublicWidgetZones.HeadHtmlTag,             // Global CSS/JS injection
            PublicWidgetZones.OrderDetailsPageOverview  // Order details page
        });
    }

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/Gifts/Configure";
    }

    /// <summary>
    /// Install the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        // Install default settings
        await _settingService.SaveSettingAsync(new GiftsSettings
        {
            GiftsCategoryId = 0,
            ExcludeCategoryId = 0,
            PercentageGifts = 10m,
            DisplayWalletDetails = true,
            GiftButtonForeground = "#FFFFFF",
            GiftButtonBackground = "#4CAF50",
            GiftButtonBorder = "#4CAF50",
            ExceedButtonForeground = "#FFFFFF",
            ExceedButtonBackground = "#DC3545",
            ExceedButtonBorder = "#DC3545",
            LockedText = "🔒 Locked",
            DescriptionDetails = @"
       <p>You can select gift products up depending on the products in your cart. 
       Add regular-priced products to unlock more gifts.</p>
       <p><b style=""color:green;"">Green</b> = Available</p> 
       <p><b style=""color: red;"">Red</b> = Locked (add regular-priced products to unlock)</p>",
            DescriptionTitle= "How to unlock your gifts"

        });

        // Activate widget
        if (!_widgetSettings.ActiveWidgetSystemNames.Contains("Ecomzen.Gifts"))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add("Ecomzen.Gifts");
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        // Add English localization resources
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Ecomzen.Gifts.Configure.Description"] = "<p>Configure your gifts settings.</p>",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsCategoryId"] = "Gift Category",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsCategoryId.Hint"] = "Select the category for gift products",
            ["Plugins.Ecomzen.Gifts.Fields.ExcludeCategoryId"] = "Wallet Exclusion Category",
            ["Plugins.Ecomzen.Gifts.Fields.ExcludeCategoryId.Hint"] = "Select the category to exclude products from the Gift Wallet",
            ["Plugins.Ecomzen.Gifts.Fields.PercentageGifts"] = "Gift Percentage",
            ["Plugins.Ecomzen.Gifts.Fields.PercentageGifts.Hint"] = "Enter the percentage for gifts (e.g., 10 for 10%)",
            ["Plugins.Ecomzen.Gifts.Fields.DisplayWalletDetails"] = "Display Wallet Details",
            ["Plugins.Ecomzen.Gifts.Fields.DisplayWalletDetails.Hint"] = "Show or hide wallet amount information to customers",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsDescription"] = "Gifts Description",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsDescription.Hint"] = "Description text shown to customers about the gift program",
            ["Plugins.Ecomzen.Gifts.Fields.TitleDescription"] = "Title Description",
            ["Plugins.Ecomzen.Gifts.Fields.TitleDescription.Hint"] = "Title text shown above the gifts description",
            ["Plugins.Ecomzen.Gifts.Fields.LockedText"] = "Locked Gift Text",
            ["Plugins.Ecomzen.Gifts.Fields.LockedText.Hint"] = "Text displayed when a gift is locked (unavailable)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonForeground"] = "Gift Button Text Color",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonForeground.Hint"] = "Color for gift button text (e.g., #FFFFFF)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBackground"] = "Gift Button Background Color",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBackground.Hint"] = "Background color for gift buttons (e.g., #4CAF50)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBorder"] = "Gift Button Border Color",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBorder.Hint"] = "Border color for gift buttons (e.g., #4CAF50)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonForeground"] = "Exceeded Wallet Button Text Color",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonForeground.Hint"] = "Color for exceeded wallet button text (e.g., #FFFFFF)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBackground"] = "Exceeded Wallet Button Background Color",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBackground.Hint"] = "Background color for exceeded wallet buttons (e.g., #DC3545)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBorder"] = "Exceeded Wallet Button Border Color",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBorder.Hint"] = "Border color for exceeded wallet buttons (e.g., #DC3545)",
            ["Plugins.Ecomzen.Gifts.GiftWallet"] = "Your Gift Wallet",
            ["Plugins.Ecomzen.Gifts.NoProducts"] = "No gifts are available at this time.",
            ["Plugins.Ecomzen.Gifts.Spent"] = "Spent",
            ["Plugins.Ecomzen.Gifts.Remaining"] = "Remaining Balance",
            ["Plugins.Ecomzen.Gifts.Dismiss"] = "Dismiss",

            ["Plugins.Ecomzen.Gifts.InsufficientWallet"] = "You need to add regular-priced products for this gift",
            ["Plugins.Ecomzen.Gifts.GlobalDiscountApplied"] = "You cannot add a gift while a global discount is applied",
            ["Plugins.Ecomzen.Gifts.ApplyingGlobalDiscount"] = "Gifts are removed when a global discount is applied",
            ["Plugins.Ecomzen.Gifts.TotalValue"] = "Total value of your gifts:",
            ["Plugins.Ecomzen.Gifts.ItemsRemovedNotification"] = "Gifts were removed because you need to add more products.",
            ["Plugins.Ecomzen.Gifts.OrderDetails.Title"] = "Gift Information",
            ["Plugins.Ecomzen.Gifts.OrderDetails.GiftValue"] = "Total Gift Value",
            ["Plugins.Ecomzen.Gifts.OrderDetails.GiftCost"] = "Gift Cost",
            ["Admin.Ecomzen.Menu"] = "Ecomzen",
            ["Admin.Ecomzen.Gifts.Menu"] = "Gift Management"
        });

        // Add French localization resources
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Ecomzen.Gifts.Configure.Description"] = "<p>Configurez les paramètres de vos cadeaux.</p>",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsCategoryId"] = "Catégorie de cadeaux",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsCategoryId.Hint"] = "Sélectionnez la catégorie des produits cadeaux",
            ["Plugins.Ecomzen.Gifts.Fields.ExcludeCategoryId"] = "Catégorie d'exclusion du portefeuille",
            ["Plugins.Ecomzen.Gifts.Fields.ExcludeCategoryId.Hint"] = "Sélectionnez la catégorie pour exclure des produits du portefeuille cadeaux",
            ["Plugins.Ecomzen.Gifts.Fields.PercentageGifts"] = "Pourcentage de cadeaux",
            ["Plugins.Ecomzen.Gifts.Fields.PercentageGifts.Hint"] = "Entrez le pourcentage pour les cadeaux (ex: 10 pour 10%)",
            ["Plugins.Ecomzen.Gifts.Fields.DisplayWalletDetails"] = "Afficher les détails du portefeuille",
            ["Plugins.Ecomzen.Gifts.Fields.DisplayWalletDetails.Hint"] = "Afficher ou masquer les informations du montant du portefeuille aux clients",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsDescription"] = "Description des cadeaux",
            ["Plugins.Ecomzen.Gifts.Fields.GiftsDescription.Hint"] = "Texte de description affiché aux clients concernant le programme de cadeaux",
            ["Plugins.Ecomzen.Gifts.Fields.TitleDescription"] = "Titre de description",
            ["Plugins.Ecomzen.Gifts.Fields.TitleDescription.Hint"] = "Texte du titre affiché au-dessus de la description des cadeaux",
            ["Plugins.Ecomzen.Gifts.Fields.LockedText"] = "Texte cadeau verrouillé",
            ["Plugins.Ecomzen.Gifts.Fields.LockedText.Hint"] = "Texte affiché lorsqu'un cadeau est verrouillé (non disponible)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonForeground"] = "Couleur du texte du bouton cadeau",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonForeground.Hint"] = "Couleur du texte du bouton cadeau (ex: #FFFFFF)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBackground"] = "Couleur de fond du bouton cadeau",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBackground.Hint"] = "Couleur de fond des boutons cadeaux (ex: #4CAF50)",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBorder"] = "Couleur de bordure du bouton cadeau",
            ["Plugins.Ecomzen.Gifts.Fields.GiftButtonBorder.Hint"] = "Couleur de bordure des boutons cadeaux (ex: #4CAF50)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonForeground"] = "Couleur du texte du bouton portefeuille dépassé",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonForeground.Hint"] = "Couleur du texte du bouton portefeuille dépassé (ex: #FFFFFF)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBackground"] = "Couleur de fond du bouton portefeuille dépassé",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBackground.Hint"] = "Couleur de fond des boutons portefeuille dépassé (ex: #DC3545)",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBorder"] = "Couleur de bordure du bouton portefeuille dépassé",
            ["Plugins.Ecomzen.Gifts.Fields.ExceedButtonBorder.Hint"] = "Couleur de bordure des boutons portefeuille dépassé (ex: #DC3545)",
            ["Plugins.Ecomzen.Gifts.GiftWallet"] = "Votre portefeuille cadeaux",
            ["Plugins.Ecomzen.Gifts.NoProducts"] = "Aucun cadeau n'est disponible pour le moment.",
            ["Plugins.Ecomzen.Gifts.Spent"] = "Dépensé",
            ["Plugins.Ecomzen.Gifts.Remaining"] = "Solde restant",
            ["Plugins.Ecomzen.Gifts.Dismiss"] = "Fermer",
            ["Plugins.Ecomzen.Gifts.InsufficientWallet"] = "Vous devez ajouter des produits à prix régulier pour ce cadeau",
            ["Plugins.Ecomzen.Gifts.GlobalDiscountApplied"] = "Vous ne pouvez pas ajouter un cadeau lorsqu'une remise globale est appliquée",
            ["Plugins.Ecomzen.Gifts.ApplyingGlobalDiscount"] = "Les cadeaux sont retirés lorsqu'une remise globale est appliquée",
            ["Plugins.Ecomzen.Gifts.TotalValue"] = "Valeur totale de vos cadeaux:",
            ["Plugins.Ecomzen.Gifts.ItemsRemovedNotification"] = "Les cadeaux ont été retirés car vous devez ajouter plus de produits.",
            ["Plugins.Ecomzen.Gifts.OrderDetails.Title"] = "Information sur les cadeaux",
            ["Plugins.Ecomzen.Gifts.OrderDetails.GiftValue"] = "Valeur totale des cadeaux",
            ["Plugins.Ecomzen.Gifts.OrderDetails.GiftCost"] = "Coût des cadeaux",
            ["Admin.Ecomzen.Menu"] = "Ecomzen",
            ["Admin.Ecomzen.Gifts.Menu"] = "Gestion des cadeaux"
        }, languageId: 2); // languageId 2 is typically French in nopCommerce

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        // Delete settings
        await _settingService.DeleteSettingAsync<GiftsSettings>();

        // Deactivate widget if it's active
        if (_widgetSettings.ActiveWidgetSystemNames.Contains("Ecomzen.Gifts"))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove("Ecomzen.Gifts");
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        // Delete localization resources
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Ecomzen.Gifts");
        await _localizationService.DeleteLocaleResourcesAsync("Admin.Ecomzen");

        await base.UninstallAsync();
    }

    #endregion
}