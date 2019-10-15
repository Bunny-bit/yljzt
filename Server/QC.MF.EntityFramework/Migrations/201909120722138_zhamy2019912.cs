namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zhamy2019912 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Renyuan1", "banji", c => c.String());
            AddColumn("dbo.Renyuan1", "xuehao", c => c.Int(nullable: false));
            DropColumn("dbo.Renyuan1", "bianji");
            DropColumn("dbo.Renyuan1", "yuehao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Renyuan1", "yuehao", c => c.Int(nullable: false));
            AddColumn("dbo.Renyuan1", "bianji", c => c.String());
            DropColumn("dbo.Renyuan1", "xuehao");
            DropColumn("dbo.Renyuan1", "banji");
        }
    }
}
