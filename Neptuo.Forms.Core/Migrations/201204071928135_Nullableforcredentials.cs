namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Nullableforcredentials : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "UserAccounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fullname = c.String(),
                        Email = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false),
                        UserRole = c.String(),
                        LocalCredentialsID = c.Int(),
                        RemoteCredentialsID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "LocalCredentials",
                c => new
                    {
                        UserAccountID = c.Int(nullable: false),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserAccountID)
                .ForeignKey("UserAccounts", t => t.UserAccountID)
                .Index(t => t.UserAccountID);
            
            CreateTable(
                "RemoteCredentials",
                c => new
                    {
                        UserAccountID = c.Int(nullable: false),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.UserAccountID)
                .ForeignKey("UserAccounts", t => t.UserAccountID)
                .Index(t => t.UserAccountID);
            
        }
        
        public override void Down()
        {
            DropIndex("RemoteCredentials", new[] { "UserAccountID" });
            DropIndex("LocalCredentials", new[] { "UserAccountID" });
            DropForeignKey("RemoteCredentials", "UserAccountID", "UserAccounts");
            DropForeignKey("LocalCredentials", "UserAccountID", "UserAccounts");
            DropTable("RemoteCredentials");
            DropTable("LocalCredentials");
            DropTable("UserAccounts");
        }
    }
}
