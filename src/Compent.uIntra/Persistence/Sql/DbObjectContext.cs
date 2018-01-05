using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.Location.Entities;
using uIntra.Core.Media;
using uIntra.Core.MigrationHistories.Sql;
using uIntra.Core.Persistence;
using uIntra.Groups.Sql;
using uIntra.Likes;
using uIntra.Navigation;
using uIntra.Notification;
using uIntra.Notification.Core.Sql;
using uIntra.Subscribe;

namespace Compent.uIntra.Persistence.Sql
{
    public class DbObjectContext : IntranetDbContext
    {
        protected Type EntityTypeConfiguration => typeof(EntityTypeConfiguration<>);

        public DbObjectContext() : this("umbracoDbDSN")
        {
        }

        public DbObjectContext(string nameOrConnectionString)
              : base(nameOrConnectionString)
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<IntranetActivityEntity> IntranetActivityEntities { get; set; }
        public DbSet<ActivityLocationEntity> ActivityLocations { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<MyLink> MyLinks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<MemberNotifierSetting> MemberNotifierSettings { get; set; }
        public DbSet<NotificationSetting> NotificationSettings { get; set; }
        public DbSet<MigrationHistory> MigrationHistories { get; set; }
        public DbSet<IntranetMediaEntity> IntranetMediaEntities { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupDocument> GroupDocuments { get; set; }
        public DbSet<GroupActivityRelation> GroupActivities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == EntityTypeConfiguration).ToList();

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}