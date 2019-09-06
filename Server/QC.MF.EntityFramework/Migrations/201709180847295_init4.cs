namespace QC.MF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VerificationCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 10),
                        PhoneNumber = c.String(nullable: false, maxLength: 11),
                        IsVerifyPass = c.Boolean(nullable: false),
                        ErrorCount = c.Int(nullable: false),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VerificationCodes");
        }
    }
}
