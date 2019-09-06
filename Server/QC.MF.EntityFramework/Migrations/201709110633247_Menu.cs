namespace QC.MF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Menu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        DisplayName = c.String(),
                        Icon = c.String(),
                        IsLeaf = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        IsVisible = c.Boolean(nullable: false),
                        RequiredPermissionName = c.String(),
                        RequiresAuthentication = c.Boolean(nullable: false),
                        Target = c.String(),
                        Url = c.String(),
                        IsSystem = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.AppBinaryObjects",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BinaryObject_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AppBinaryObjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenantId = c.Int(),
                        Bytes = c.Binary(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BinaryObject_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Menus");
        }
    }
}
