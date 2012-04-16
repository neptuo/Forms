namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddPublicIdentifierToFieldDefinition : DbMigration
    {
        public override void Up()
        {
            AddColumn("FieldDefinitions", "PublicIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("FieldDefinitions", "PublicIdentifier");
        }
    }
}
