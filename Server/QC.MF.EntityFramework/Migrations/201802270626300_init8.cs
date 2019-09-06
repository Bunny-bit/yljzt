namespace QC.MF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class init8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppEditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.String(maxLength: 10),
                        AboutUrl = c.String(maxLength: 500),
                        InstallationPackage = c.Guid(),
                        IsMandatoryUpdate = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Describe = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        ItunesUrl = c.String(maxLength: 500),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AndroidAppEdition_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_AppEdition_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_IOSAppEdition_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(maxLength: 512),
                        FilePath = c.String(maxLength: 512),
                        TempFilePath = c.String(maxLength: 512),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebFiles");
            DropTable("dbo.AppEditions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AndroidAppEdition_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_AppEdition_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_IOSAppEdition_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
