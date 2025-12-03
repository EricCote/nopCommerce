using System.Diagnostics;
using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Ecomzen.Plugin.Misc.Gifts.Domains;

namespace Ecomzen.Plugin.Misc.Gifts.Migrations;

[NopMigration("2025/01/13 12:00:00", "Ecomzen.Gifts base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{ 
    /// <summary>
    /// Collect the UP migration expressions
    /// </summary>
    public override void Up()
    {
        Debug.WriteLine("------------------------ Migration Up");
        Create.TableFor<AlloGreeting>();

        // Insert default data
        Insert.IntoTable(nameof(AlloGreeting))
            .Row(new { Name = "World" });
    }
}