namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddPublicIdentifierToUserAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("UserAccounts", "PublicIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("UserAccounts", "PublicIdentifier");
        }
    }
}
