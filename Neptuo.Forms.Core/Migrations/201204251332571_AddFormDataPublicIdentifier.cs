namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddFormDataPublicIdentifier : DbMigration
    {
        public override void Up()
        {
            AddColumn("FormDatas", "PublicIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("FormDatas", "PublicIdentifier");
        }
    }
}
