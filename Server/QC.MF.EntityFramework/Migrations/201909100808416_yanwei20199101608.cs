namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yanwei20199101608 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Renyua1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Xueyua = c.String(),
                        Xuehao = c.Int(nullable: false),
                        Banji = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Renyua1");
        }
    }
}
