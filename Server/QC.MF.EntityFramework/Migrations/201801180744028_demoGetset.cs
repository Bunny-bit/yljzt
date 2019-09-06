namespace QC.MF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class demoGetset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileSettingDemoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileSize = c.Long(nullable: false),
                        FileExtension = c.String(),
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
                    { "DynamicFilter_FileSettingDemo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FileSettingDemoes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FileSettingDemo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
