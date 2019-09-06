namespace QC.MF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class init5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppChatMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        TenantId = c.Int(),
                        TargetUserId = c.Long(nullable: false),
                        TargetTenantId = c.Int(),
                        Message = c.String(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        Side = c.Int(nullable: false),
                        ReadState = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ChatMessage_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppFriendships",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        TenantId = c.Int(),
                        FriendUserId = c.Long(nullable: false),
                        FriendTenantId = c.Int(),
                        FriendUserName = c.String(nullable: false, maxLength: 32),
                        FriendTenancyName = c.String(),
                        FriendProfilePictureId = c.Guid(),
                        State = c.Int(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Friendship_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppFriendships",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Friendship_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AppChatMessages",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ChatMessage_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
