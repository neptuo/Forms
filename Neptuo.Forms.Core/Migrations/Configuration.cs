using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Neptuo.Forms.Core.DataAccess;
using Neptuo.Forms.Core.Utils;

namespace Neptuo.Forms.Core.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataContext context)
        {
            UserAccount admin = null;
            if (context.Users.FirstOrDefault(u => u.Fullname == FormsCore.AdminFullname) == null)
            {
                admin = context.Users.Add(new UserAccount
                {
                    Fullname = FormsCore.AdminFullname,
                    Created = DateTime.Now,
                    Enabled = true,
                    UserRole = UserRole.Admin,
                    PublicIdentifier = HashHelper.ComputePublicIdentifier(typeof(UserAccount).Name, FormsCore.AdminFullname),
                    Email = FormsCore.AdminEmail,
                    LocalCredentials = new LocalCredentials
                    {
                        Username = FormsCore.AdminUsername,
                        Password = HashHelper.ComputePassword(FormsCore.AdminUsername, FormsCore.AdminPassword)
                    }
                });
            } else {
                admin = context.Users.FirstOrDefault(u => u.LocalCredentials != null && u.LocalCredentials.Username == FormsCore.AdminUsername);
            }

            //TODO: Create app forms+projects+data
            if (context.Projects.Where(p => p.Name == "FormService").Count() == 0)
            {
                //Project
                Project project = context.Projects.Add(new Project
                {
                    Name = "FormService",
                    Created = DateTime.Now,
                    OwnerUserID = admin.ID
                });

                //FormDefinitions
                FormDefinition miniForum = context.FormDefinitions.Add(new FormDefinition
                {
                    Name = "Mini-forum",
                    ProjectID = project.ID,
                    PublicContent = true,
                    PublicIdentifier = "66e9da18r5d92",
                    FormType = FormType.Form
                });
                FieldDefinition miniMessage = context.FieldDefinitions.Add(new FieldDefinition
                {
                    Name = "Message",
                    FieldType = FieldType.StringField,
                    Required = true,
                    FormDefinitionID = miniForum.ID,
                    PublicIdentifier = "27460bddaaddd"
                });
                FieldDefinition miniAuthor = context.FieldDefinitions.Add(new FieldDefinition
                {
                    Name = "Author",
                    FieldType = FieldType.StringField,
                    Required = false,
                    FormDefinitionID = miniForum.ID,
                    PublicIdentifier = "17734ff6tf0d0"
                });
            }
        }
    }
}
