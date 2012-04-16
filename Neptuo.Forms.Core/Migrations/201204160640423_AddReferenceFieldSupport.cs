namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddReferenceFieldSupport : DbMigration
    {
        public override void Up()
        {
            AddColumn("FieldDefinitions", "ReferenceFormID", c => c.Int());
            AddColumn("FieldDefinitions", "ReferenceDisplayFieldID", c => c.Int());
            AddForeignKey("FieldDefinitions", "ReferenceFormID", "FormDefinitions", "ID");
            AddForeignKey("FieldDefinitions", "ReferenceDisplayFieldID", "FieldDefinitions", "ID");
            CreateIndex("FieldDefinitions", "ReferenceFormID");
            CreateIndex("FieldDefinitions", "ReferenceDisplayFieldID");
        }
        
        public override void Down()
        {
            DropIndex("FieldDefinitions", new[] { "ReferenceDisplayFieldID" });
            DropIndex("FieldDefinitions", new[] { "ReferenceFormID" });
            DropForeignKey("FieldDefinitions", "ReferenceDisplayFieldID", "FieldDefinitions");
            DropForeignKey("FieldDefinitions", "ReferenceFormID", "FormDefinitions");
            DropColumn("FieldDefinitions", "ReferenceDisplayFieldID");
            DropColumn("FieldDefinitions", "ReferenceFormID");
        }
    }
}
