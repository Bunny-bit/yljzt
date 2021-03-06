namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xiemiaofan2019923 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Daans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DaanId = c.String(),
                        DaanNeirong = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Renyua1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Xueyua = c.String(),
                        Xuehao = c.Int(nullable: false),
                        Banji = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tikus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TiHao = c.Int(nullable: false),
                        TiMu = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Xuanxiangs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimuId = c.Int(nullable: false),
                        Name = c.String(),
                        Neirong = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Xueyuans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Xueyuans");
            DropTable("dbo.Xuanxiangs");
            DropTable("dbo.Tikus");
            DropTable("dbo.Renyua1");
            DropTable("dbo.Daans");
        }
    }
}
