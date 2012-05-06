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

        public DbSet<FormData> FormData { get; set; }
        public DbSet<FieldData> FieldData { get; set; }

        public DbSet<ProjectInvitation> ProjectInvitations { get; set; }

        public DataContext()
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>().HasMany(p => p.Managers).WithMany().Map(mc =>
            {
                mc.ToTable("ProjectManagers");
                mc.MapLeftKey("ProjectID");
                mc.MapRightKey("UserAccountID");
            });
            modelBuilder.Entity<Project>().HasMany(p => p.Readers).WithMany().Map(mc =>
            {
                mc.ToTable("ProjectReaders");
                mc.MapLeftKey("ProjectID");
                mc.MapRightKey("UserAccountID");
            });
        }
    }
}
