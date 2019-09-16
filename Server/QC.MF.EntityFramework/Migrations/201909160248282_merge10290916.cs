namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge10290916 : DbMigration
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
                        Banji = c.String(),
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
            DropTable("dbo.Renyua1");
        }
    }
}
