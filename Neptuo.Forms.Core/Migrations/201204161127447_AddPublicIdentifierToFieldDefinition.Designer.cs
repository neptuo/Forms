// <auto-generated />
namespace Neptuo.Forms.Core.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    
    public sealed partial class AddPublicIdentifierToFieldDefinition : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201204161127447_AddPublicIdentifierToFieldDefinition"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return "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"; }
        }
    }
}
