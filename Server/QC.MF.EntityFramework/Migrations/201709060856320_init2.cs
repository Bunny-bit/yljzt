namespace QC.MF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppBinaryObjects",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BinaryObject_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
