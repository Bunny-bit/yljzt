namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lichunlin201910 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Xueyuans", "Name", c => c.String());
            DropColumn("dbo.Xueyuans", "Sname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Xueyuans", "Sname", c => c.String());
            DropColumn("dbo.Xueyuans", "Name");
        }
    }
}
