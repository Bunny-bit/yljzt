namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xiemf20190919 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Xuanxiangs", "Name", c => c.String());
            DropColumn("dbo.Xuanxiangs", "ANAME");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Xuanxiangs", "ANAME", c => c.String());
            DropColumn("dbo.Xuanxiangs", "Name");
        }
    }
}
