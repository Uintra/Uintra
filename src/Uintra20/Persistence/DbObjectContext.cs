using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using Uintra20.Core.Activity;
using Uintra20.Core.Comments;
using Uintra20.Core.Groups.Sql;
using Uintra20.Core.Likes;
using Uintra20.Core.LinkPreview.Sql;
using Uintra20.Core.Location.Sql;
using Uintra20.Core.Media;
using Uintra20.Core.Media.Sql;
using Uintra20.Core.MigrationHistories.Sql;
using Uintra20.Core.Navigation.Sql;
using Uintra20.Core.Notification;
using Uintra20.Core.Notification.Sql;
using Uintra20.Core.Permissions.Sql;
using Uintra20.Core.Subscribe.Sql;
using Uintra20.Core.Tagging.UserTags.Sql;

namespace Uintra20.Persistence
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
        public DbSet<ActivitySubscribeSetting> ActivitySubscribeSettings { get; set; }
        public DbSet<UserTagRelation> UserTagRelations { get; set; }

        public DbSet<LinkPreviewEntity> LinkPreviews { get; set; }
        public DbSet<CommentToLinkPreviewEntity> CommentToLinkPreviews { get; set; }
        public DbSet<ActivityToLinkPreviewEntity> ActivityToLinkPreviews { get; set; }
        public DbSet<VideoConvertationLog> VideoConvertationLog { get; set; }

        public DbSet<PermissionEntity> Permissions { get; set; }

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

            var convention = new AttributeToColumnAnnotationConvention<DefaultValueAttribute, string>("SqlDefaultValue", (p, attributes) => attributes.SingleOrDefault().Value.ToString());
            modelBuilder.Conventions.Add(convention);
        }
    }
}