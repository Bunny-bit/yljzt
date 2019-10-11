namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zhamy20199111123 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Renyuan1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        xingming = c.String(),
                        bianji = c.String(),
                        yuehao = c.Int(nullable: false),
                        xueyuan = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Renyuan1");
        }
    }
}
