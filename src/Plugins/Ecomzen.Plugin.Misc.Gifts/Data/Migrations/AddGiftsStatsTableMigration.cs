using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Ecomzen.Plugin.Misc.Gifts.Domain;

namespace Ecomzen.Plugin.Misc.Gifts.Data.Migrations;

[NopMigration("2024/01/16 00:00:00", "Ecomzen.Gifts schema", MigrationProcessType.Installation)]
public class AddGiftsStatsTableMigration : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.TableFor<GiftsStats>();
    }
}
