namespace QC.MF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class demo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Demoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LongText = c.String(),
                        Group = c.String(),
                        IsActivate = c.Boolean(nullable: false),
                        Sort = c.Double(nullable: false),
                        Weight = c.Decimal(precision: 18, scale: 2),
                        PublishTime = c.DateTime(),
                        Avatar = c.Binary(),
                        Location_Longitude = c.String(),
                        Location_Latitude = c.String(),
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
                    { "DynamicFilter_Demo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Demoes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Demo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
