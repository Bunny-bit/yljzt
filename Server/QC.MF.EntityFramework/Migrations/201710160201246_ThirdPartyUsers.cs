namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdPartyUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ThirdPartyUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        OpenId = c.String(),
                        AccessToken = c.String(),
                        NickName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ThirdPartyUsers");
        }
    }
}
