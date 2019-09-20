namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xiemiaofan20199161658 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Timus", newName: "Tikus");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Tikus", newName: "Timus");
        }
    }
}
