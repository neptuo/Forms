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
            if (context.Users.FirstOrDefault(u => u.Fullname == FormsCore.AdminFullname) == null)
            {
                context.Users.AddOrUpdate(new UserAccount
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

                //TODO: Create app forms+projects+data
            }
        }
    }
}
