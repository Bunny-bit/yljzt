namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "ProfilePictureId", c => c.Guid());
            AddColumn("dbo.AbpUsers", "ShouldChangePasswordOnNextLogin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "ShouldChangePasswordOnNextLogin");
            DropColumn("dbo.AbpUsers", "ProfilePictureId");
        }
    }
}
