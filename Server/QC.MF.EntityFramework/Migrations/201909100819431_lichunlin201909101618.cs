namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lichunlin201909101618 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Xueyuans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sname = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Xueyuans");
        }
    }
}
