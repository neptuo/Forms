namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddFormFieldDataHierarchy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "FormDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Tag = c.String(),
                        FormDefinitionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FormDefinitions", t => t.FormDefinitionID, cascadeDelete: true)
                .Index(t => t.FormDefinitionID);
            
            CreateTable(
                "FieldDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FormDataID = c.Int(nullable: false),
                        FieldDefinitionID = c.Int(nullable: false),
                        DoubleData = c.Double(),
                        StringData = c.String(),
                        BoolData = c.Boolean(),
                        Filename = c.String(),
                        MimeType = c.String(),
                        LocalFilename = c.String(),
                        ReferenceDataID = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FieldDefinitions", t => t.FieldDefinitionID, cascadeDelete: true)
                .ForeignKey("FormDatas", t => t.ReferenceDataID, cascadeDelete: false)
                .ForeignKey("FormDatas", t => t.FormDataID, cascadeDelete: false)
                .Index(t => t.FieldDefinitionID)
                .Index(t => t.ReferenceDataID)
                .Index(t => t.FormDataID);
            
        }
        
        public override void Down()
        {
            DropIndex("FieldDatas", new[] { "FormDataID" });
            DropIndex("FieldDatas", new[] { "ReferenceDataID" });
            DropIndex("FieldDatas", new[] { "FieldDefinitionID" });
            DropIndex("FormDatas", new[] { "FormDefinitionID" });
            DropForeignKey("FieldDatas", "FormDataID", "FormDatas");
            DropForeignKey("FieldDatas", "ReferenceDataID", "FormDatas");
            DropForeignKey("FieldDatas", "FieldDefinitionID", "FieldDefinitions");
            DropForeignKey("FormDatas", "FormDefinitionID", "FormDefinitions");
            DropTable("FieldDatas");
            DropTable("FormDatas");
        }
    }
}
