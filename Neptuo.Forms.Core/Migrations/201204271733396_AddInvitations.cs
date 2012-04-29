namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddInvitations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ProjectInvitations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        TargetUserID = c.Int(nullable: false),
                        TargetProjectID = c.Int(nullable: false),
                        OwnerUserID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("UserAccounts", t => t.TargetUserID, cascadeDelete: false)
                .ForeignKey("Projects", t => t.TargetProjectID, cascadeDelete: false)
                .ForeignKey("UserAccounts", t => t.OwnerUserID, cascadeDelete: false)
                .Index(t => t.TargetUserID)
                .Index(t => t.TargetProjectID)
                .Index(t => t.OwnerUserID);
            
        }
        
        public override void Down()
        {
            DropIndex("ProjectInvitations", new[] { "OwnerUserID" });
            DropIndex("ProjectInvitations", new[] { "TargetProjectID" });
            DropIndex("ProjectInvitations", new[] { "TargetUserID" });
            DropForeignKey("ProjectInvitations", "OwnerUserID", "UserAccounts");
            DropForeignKey("ProjectInvitations", "TargetProjectID", "Projects");
            DropForeignKey("ProjectInvitations", "TargetUserID", "UserAccounts");
            DropTable("ProjectInvitations");
        }
    }
}
