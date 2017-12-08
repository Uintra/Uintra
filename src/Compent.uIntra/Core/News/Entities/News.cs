using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Groups;
using uIntra.Likes;
using uIntra.News;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.News.Entities
{
    public class News : NewsBase, IFeedItem, ICommentable, ILikeable, ISubscribable, IGroupActivity
    {
        [JsonIgnore]
        public DateTime SortDate => PublishDate;
        [JsonIgnore]
        public IEnumerable<LikeModel> Likes { get; set; }
        [JsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
        [JsonIgnore]
        public IEnumerable<global::uIntra.Subscribe.Subscribe> Subscribers { get; set; }

        public Guid? GroupId { get; set; }

        [JsonIgnore]
        public bool IsReadOnly { get; set; }

        public News()
        {
            var migration = DependencyResolver.Current.GetService<global::Compent.uIntra.Installer.Migrations.OldUiNotificationMigration>();
            migration.Execute();
        }
    }
}