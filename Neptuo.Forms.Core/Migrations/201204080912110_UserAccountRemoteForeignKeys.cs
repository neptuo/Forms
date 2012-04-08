namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserAccountRemoteForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropColumn("UserAccounts", "LocalCredentialsID");
            DropColumn("UserAccounts", "RemoteCredentialsID");
        }
        
        public override void Down()
        {
            AddColumn("UserAccounts", "RemoteCredentialsID", c => c.Int());
            AddColumn("UserAccounts", "LocalCredentialsID", c => c.Int());
        }
    }
}
