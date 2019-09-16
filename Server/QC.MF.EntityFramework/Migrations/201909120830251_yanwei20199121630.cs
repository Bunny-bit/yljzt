namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yanwei20199121630 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Renyua1", "Banji", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Renyua1", "Banji", c => c.Int(nullable: false));
        }
    }
}
