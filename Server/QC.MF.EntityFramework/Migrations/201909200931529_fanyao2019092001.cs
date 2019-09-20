namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fanyao2019092001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Daans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RenyuanId = c.Int(nullable: false),
                        TimuId = c.Int(nullable: false),
                        XuanxiangId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Daans");
        }
    }
}
