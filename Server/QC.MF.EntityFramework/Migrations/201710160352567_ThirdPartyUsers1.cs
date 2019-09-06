namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdPartyUsers1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThirdPartyUsers", "ThirdParty", c => c.String());
            AddColumn("dbo.ThirdPartyUsers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThirdPartyUsers", "Name");
            DropColumn("dbo.ThirdPartyUsers", "ThirdParty");
        }
    }
}
