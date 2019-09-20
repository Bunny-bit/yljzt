namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xiemf20190917 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Xuanxiangs", "TimuId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Xuanxiangs", "TimuId");
        }
    }
}
