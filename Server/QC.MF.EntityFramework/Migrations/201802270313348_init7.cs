namespace QC.MF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class init7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppStartPages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Platform = c.Int(),
                        Width_Px = c.Int(),
                        High_Px = c.Int(),
                        Url = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AppStartPage_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppStartPages",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AppStartPage_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
