namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xiemiaofan20190916 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Timus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TiMu = c.String(),
                        TiHao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Xuanxiangs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ANAME = c.String(),
                        Neirong = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Xuanxiangs");
            DropTable("dbo.Timus");
        }
    }
}
