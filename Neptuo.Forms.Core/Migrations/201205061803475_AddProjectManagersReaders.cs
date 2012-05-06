namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectManagersReaders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ProjectManagers",
                c => new
                    {
                        ProjectID = c.Int(nullable: false),
                        UserAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.UserAccountID })
                .ForeignKey("Projects", t => t.ProjectID, cascadeDelete: false)
                .ForeignKey("UserAccounts", t => t.UserAccountID, cascadeDelete: false)
                .Index(t => t.ProjectID)
                .Index(t => t.UserAccountID);
            
            CreateTable(
                "ProjectReaders",
                c => new
                    {
                        ProjectID = c.Int(nullable: false),
                        UserAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectID, t.UserAccountID })
                .ForeignKey("Projects", t => t.ProjectID, cascadeDelete: false)
                .ForeignKey("UserAccounts", t => t.UserAccountID, cascadeDelete: false)
                .Index(t => t.ProjectID)
                .Index(t => t.UserAccountID);
            
        }
        
        public override void Down()
        {
            DropIndex("ProjectReaders", new[] { "UserAccountID" });
            DropIndex("ProjectReaders", new[] { "ProjectID" });
            DropIndex("ProjectManagers", new[] { "UserAccountID" });
            DropIndex("ProjectManagers", new[] { "ProjectID" });
            DropForeignKey("ProjectReaders", "UserAccountID", "UserAccounts");
            DropForeignKey("ProjectReaders", "ProjectID", "Projects");
            DropForeignKey("ProjectManagers", "UserAccountID", "UserAccounts");
            DropForeignKey("ProjectManagers", "ProjectID", "Projects");
            DropTable("ProjectReaders");
            DropTable("ProjectManagers");
        }
    }
}
