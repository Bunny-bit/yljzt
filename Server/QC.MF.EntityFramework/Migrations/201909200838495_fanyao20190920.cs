namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fanyao20190920 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Renyua1", "Xuehao", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Renyua1", "Xuehao", c => c.Int(nullable: false));
        }
    }
}
