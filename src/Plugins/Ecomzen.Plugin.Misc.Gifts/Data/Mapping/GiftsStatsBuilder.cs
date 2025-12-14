using System.Data;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Orders;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Ecomzen.Plugin.Misc.Gifts.Domain;

namespace Ecomzen.Plugin.Misc.Gifts.Data.Mapping;

/// <summary>
/// Represents a gifts stats entity builder
/// </summary>
public class GiftsStatsBuilder : NopEntityBuilder<GiftsStats>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(GiftsStats.OrderId))
                .AsInt32()
                .NotNullable()
                .PrimaryKey()
                .ForeignKey<Order>(onDelete: Rule.Cascade)
            .WithColumn(nameof(GiftsStats.GiftValue))
                .AsDecimal(18, 4)
                .NotNullable()
            .WithColumn(nameof(GiftsStats.GiftCost))
                .AsDecimal(18, 4)
                .NotNullable();
    }

    #endregion
}
