namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddArticle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Articles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Culture = c.String(),
                        Modified = c.DateTime(nullable: false),
                        AuthorUserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("UserAccounts", t => t.AuthorUserID)
                .Index(t => t.AuthorUserID);
            
        }
        
        public override void Down()
        {
            DropIndex("Articles", new[] { "AuthorUserID" });
            DropForeignKey("Articles", "AuthorUserID", "UserAccounts");
            DropTable("Articles");
        }
    }
}
