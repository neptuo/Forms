namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddFormFieldData : DbMigration
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
                "DoubleFieldDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Data = c.Double(nullable: false),
                        FormDataID = c.Int(nullable: false),
                        FieldDefinitionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FormDatas", t => t.FormDataID, cascadeDelete: true)
                .ForeignKey("FieldDefinitions", t => t.FieldDefinitionID, cascadeDelete: false)
                .Index(t => t.FormDataID)
                .Index(t => t.FieldDefinitionID);
            
            CreateTable(
                "StringFieldDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Data = c.String(),
                        FormDataID = c.Int(nullable: false),
                        FieldDefinitionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FormDatas", t => t.FormDataID, cascadeDelete: true)
                .ForeignKey("FieldDefinitions", t => t.FieldDefinitionID, cascadeDelete: false)
                .Index(t => t.FormDataID)
                .Index(t => t.FieldDefinitionID);
            
            CreateTable(
                "BoolFieldDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Data = c.Boolean(nullable: false),
                        FormDataID = c.Int(nullable: false),
                        FieldDefinitionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FormDatas", t => t.FormDataID, cascadeDelete: true)
                .ForeignKey("FieldDefinitions", t => t.FieldDefinitionID, cascadeDelete: false)
                .Index(t => t.FormDataID)
                .Index(t => t.FieldDefinitionID);
            
            CreateTable(
                "FileFieldDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Filename = c.String(),
                        MimeType = c.String(),
                        LocalFilename = c.String(),
                        FormDataID = c.Int(nullable: false),
                        FieldDefinitionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FormDatas", t => t.FormDataID, cascadeDelete: true)
                .ForeignKey("FieldDefinitions", t => t.FieldDefinitionID, cascadeDelete: false)
                .Index(t => t.FormDataID)
                .Index(t => t.FieldDefinitionID);
            
            CreateTable(
                "ReferenceFieldDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DataID = c.Int(nullable: false),
                        FormDataID = c.Int(nullable: false),
                        FieldDefinitionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FormDatas", t => t.DataID, cascadeDelete: false)
                .ForeignKey("FormDatas", t => t.FormDataID, cascadeDelete: true)
                .ForeignKey("FieldDefinitions", t => t.FieldDefinitionID, cascadeDelete: false)
                .Index(t => t.DataID)
                .Index(t => t.FormDataID)
                .Index(t => t.FieldDefinitionID);
            
        }
        
        public override void Down()
        {
            DropIndex("ReferenceFieldDatas", new[] { "FieldDefinitionID" });
            DropIndex("ReferenceFieldDatas", new[] { "FormDataID" });
            DropIndex("ReferenceFieldDatas", new[] { "DataID" });
            DropIndex("FileFieldDatas", new[] { "FieldDefinitionID" });
            DropIndex("FileFieldDatas", new[] { "FormDataID" });
            DropIndex("BoolFieldDatas", new[] { "FieldDefinitionID" });
            DropIndex("BoolFieldDatas", new[] { "FormDataID" });
            DropIndex("StringFieldDatas", new[] { "FieldDefinitionID" });
            DropIndex("StringFieldDatas", new[] { "FormDataID" });
            DropIndex("FormDatas", new[] { "FormDefinitionID" });
            DropIndex("DoubleFieldDatas", new[] { "FieldDefinitionID" });
            DropIndex("DoubleFieldDatas", new[] { "FormDataID" });
            DropForeignKey("ReferenceFieldDatas", "FieldDefinitionID", "FieldDefinitions");
            DropForeignKey("ReferenceFieldDatas", "FormDataID", "FormDatas");
            DropForeignKey("ReferenceFieldDatas", "DataID", "FormDatas");
            DropForeignKey("FileFieldDatas", "FieldDefinitionID", "FieldDefinitions");
            DropForeignKey("FileFieldDatas", "FormDataID", "FormDatas");
            DropForeignKey("BoolFieldDatas", "FieldDefinitionID", "FieldDefinitions");
            DropForeignKey("BoolFieldDatas", "FormDataID", "FormDatas");
            DropForeignKey("StringFieldDatas", "FieldDefinitionID", "FieldDefinitions");
            DropForeignKey("StringFieldDatas", "FormDataID", "FormDatas");
            DropForeignKey("FormDatas", "FormDefinitionID", "FormDefinitions");
            DropForeignKey("DoubleFieldDatas", "FieldDefinitionID", "FieldDefinitions");
            DropForeignKey("DoubleFieldDatas", "FormDataID", "FormDatas");
            DropTable("ReferenceFieldDatas");
            DropTable("FileFieldDatas");
            DropTable("BoolFieldDatas");
            DropTable("StringFieldDatas");
            DropTable("FormDatas");
            DropTable("DoubleFieldDatas");
        }
    }
}
