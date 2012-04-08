using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<LocalCredentials> LocalCredentials { get; set; }
        public DbSet<RemoteCredentials> RemoteCredentials { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<FormDefinition> FormDefinitions { get; set; }
        public DbSet<FieldDefinition> FieldDefinitions { get; set; }
    }
}
