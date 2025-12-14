# Gift Value Widget for Order Details Page

## Overview
This implementation adds a widget to display gift value information from the `GiftsStats` table on the customer's order details page when they view their order in "My Account > Orders".

## Files Created

### 1. **OrderDetailsGiftsViewComponent.cs**
- Location: `Plugins/Ecomzen.Plugin.Misc.Gifts/Components/OrderDetailsGiftsViewComponent.cs`
- Purpose: View component that fetches gift statistics for the order and prepares the display model
- Key Features:
  - Retrieves `GiftsStats` data for the order
  - Formats prices using the `IPriceFormatter` service
  - Only displays widget if gift data exists
  - Receives the `OrderDetailsModel` as additionalData from the widget zone

### 2. **OrderDetailsGiftsModel.cs**
- Location: `Plugins/Ecomzen.Plugin.Misc.Gifts/Models/OrderDetailsGiftsModel.cs`
- Purpose: Model for displaying gift statistics
- Properties:
  - `GiftValueFormatted`: Formatted string for display (e.g., "$50.00")
  - `GiftCostFormatted`: Formatted string for display
  - `GiftValue`: Raw decimal value
  - `GiftCost`: Raw decimal value

### 3. **OrderDetailsGifts.cshtml**
- Location: `Plugins/Ecomzen.Plugin.Misc.Gifts/Views/OrderDetailsGifts.cshtml`
- Purpose: Razor view that renders the gift information
- Display:
  - Shows a "Gift Information" section
  - Displays "Total Gift Value" if GiftValue > 0
  - Displays "Gift Cost" if GiftCost > 0
  - Uses standard nopCommerce styling consistent with other order details sections

## Files Modified

### 1. **GiftsPlugin.cs**
Updated to register the new widget zone and view component:

**Widget Zone Added:**
- `PublicWidgetZones.OrderDetailsPageOverview` - This zone appears in the order details page after the order overview section

**GetWidgetViewComponent Method:**
- Returns `OrderDetailsGiftsViewComponent` when the widget zone is `OrderDetailsPageOverview`

**Localization Resources Added:**
- English:
  - `Plugins.Ecomzen.Gifts.OrderDetails.Title` = "Gift Information"
  - `Plugins.Ecomzen.Gifts.OrderDetails.GiftValue` = "Total Gift Value"
  - `Plugins.Ecomzen.Gifts.OrderDetails.GiftCost` = "Gift Cost"
  
- French:
  - `Plugins.Ecomzen.Gifts.OrderDetails.Title` = "Information sur les cadeaux"
  - `Plugins.Ecomzen.Gifts.OrderDetails.GiftValue` = "Valeur totale des cadeaux"
  - `Plugins.Ecomzen.Gifts.OrderDetails.GiftCost` = "Coût des cadeaux"

### 2. **Ecomzen.Plugin.Misc.Gifts.csproj**
- Added `OrderDetailsGifts.cshtml` to the project file
- Configured to copy to output directory

## How It Works

1. **Widget Registration**: The plugin registers for the `OrderDetailsPageOverview` widget zone, which is invoked on the order details page.

2. **Data Retrieval**: When the order details page is loaded:
   - The widget is invoked with the `OrderDetailsModel` as additional data
   - The view component extracts the order ID from the model
   - It calls `IGiftsStatsService.GetGiftsStatsByOrderIdAsync()` to fetch gift statistics

3. **Display Logic**:
   - If no gift stats exist or both values are zero, nothing is displayed
   - If gift data exists, it's formatted and displayed in a section similar to other order details sections

4. **Localization**: All text is localized and supports both English and French.

## Widget Zone Location

The `OrderDetailsPageOverview` widget zone appears on the order details page (`/Order/Details/{orderId}`) in the customer's account area, specifically after the order overview information (order number, date, status, total).

This placement ensures customers can easily see:
- The total value of gifts they received
- The cost basis of those gifts
- This information alongside their order summary

## Testing

To test this feature:

1. **Build and Deploy**: The solution has been built successfully
2. **Place an Order**: Create an order with gift products (products in the gifts category)
3. **View Order Details**: Navigate to My Account > Orders > View Details
4. **Verify Display**: The gift information section should appear showing the gift value and cost

## Benefits

- **Customer Transparency**: Customers can see the value of gifts they received
- **Order History**: Gift information is preserved with each order
- **Multilingual Support**: Works in both English and French
- **Consistent UI**: Follows nopCommerce design patterns and styling
- **Performance**: Only queries gift stats when needed, no overhead if no gifts exist
