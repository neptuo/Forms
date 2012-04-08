namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectFormDefinitionFieldDefinition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Created = c.DateTime(nullable: false),
                        OwnerUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("UserAccounts", t => t.OwnerUserID, cascadeDelete: true)
                .Index(t => t.OwnerUserID);
            
            CreateTable(
                "FormDefinitions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PublicIdentifier = c.String(),
                        FormType = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        PublicContent = c.Boolean(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "FieldDefinitions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FieldType = c.Int(nullable: false),
                        Required = c.Boolean(nullable: false),
                        FormDefinitionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("FormDefinitions", t => t.FormDefinitionID, cascadeDelete: true)
                .Index(t => t.FormDefinitionID);
            
        }
        
        public override void Down()
        {
            DropIndex("FieldDefinitions", new[] { "FormDefinitionID" });
            DropIndex("FormDefinitions", new[] { "ProjectID" });
            DropIndex("Projects", new[] { "OwnerUserID" });
            DropForeignKey("FieldDefinitions", "FormDefinitionID", "FormDefinitions");
            DropForeignKey("FormDefinitions", "ProjectID", "Projects");
            DropForeignKey("Projects", "OwnerUserID", "UserAccounts");
            DropTable("FieldDefinitions");
            DropTable("FormDefinitions");
            DropTable("Projects");
        }
    }
}
