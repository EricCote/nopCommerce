using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Ecomzen.Plugin.Misc.Gifts.Domains;

namespace Ecomzen.Plugin.Misc.Gifts.Mapping.Builders;

public class AlloGreetingBuilder : NopEntityBuilder<AlloGreeting>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(AlloGreeting.Name))
            .AsString(255)
            .NotNullable();
    }

    #endregion
}