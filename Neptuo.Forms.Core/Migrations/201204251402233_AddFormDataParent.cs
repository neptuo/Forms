namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddFormDataParent : DbMigration
    {
        public override void Up()
        {
            AddColumn("FormDatas", "ParentFormDataID", c => c.Int());
            AddForeignKey("FormDatas", "ParentFormDataID", "FormDatas", "ID");
            CreateIndex("FormDatas", "ParentFormDataID");
        }
        
        public override void Down()
        {
            DropIndex("FormDatas", new[] { "ParentFormDataID" });
            DropForeignKey("FormDatas", "ParentFormDataID", "FormDatas");
            DropColumn("FormDatas", "ParentFormDataID");
        }
    }
}
