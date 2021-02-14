using Compent.CommandBus;
using Uintra.Core.Search.Entities;
using Uintra.Core.Commands;
using Uintra.Core.Member.CommandBus;
using Uintra.Core.Member.Commands;
using Uintra.Features.Comments.CommandBus;
using Uintra.Features.Comments.CommandBus.Commands;
using Uintra.Features.Events.Handlers;
using Uintra.Features.Groups.CommandBus;
using Uintra.Features.Groups.CommandBus.Commands;
using Uintra.Features.Likes.CommandBus;
using Uintra.Features.Likes.CommandBus.Commands;
using Uintra.Features.Media.Video.Commands;
using Uintra.Features.Media.Video.Handlers;
using Uintra.Features.News.Handlers;
using Uintra.Features.Search.CommandBus;
using Uintra.Features.Social.Handlers;

namespace Uintra
{
    public class CommandBusConfiguration : CommandBindingProviderBase
    {
        protected override BindingConfiguration ConfigureBindings(BindingBuilder builder)
        {
            ConfigureLikeBindings(builder);
            ConfigureCommentBindings(builder);
            ConfigureGroupBindings(builder);
            ConfigureMediaBindings(builder);

            builder.HandleCommand<MemberChanged>()
                .WithHandle<MemberHandle<SearchableMember>>();
            builder.HandleCommand<MembersChanged>()
                .WithHandle<MemberHandle<SearchableMember>>();

            builder.HandleCommand<MentionCommand>()
                .WithHandle<MentionHandle>();

            return builder.Build();
        }

        private static void ConfigureLikeBindings(BindingBuilder builder)
        {
            builder.HandleCommand<AddLikeCommand>()
                .WithHandle<LikeHandle>()
                .WithHandle<LikeNotificationHandle>();
            
            builder.HandleCommand<RemoveLikeCommand>()
                .WithHandle<LikeHandle>();
        }

        private static void ConfigureCommentBindings(BindingBuilder builder)
        {
            builder.HandleCommand<AddCommentCommand>()
                .WithHandle<CommentHandle>()
                .WithHandle<CommentNotificationHandle>();

            builder.HandleCommand<EditCommentCommand>()
                .WithHandle<CommentHandle>()
                .WithHandle<CommentNotificationHandle>();

            builder.HandleCommand<RemoveCommentCommand>()
                .WithHandle<CommentHandle>();
        }

        private static void ConfigureGroupBindings(BindingBuilder builder)
        {
            builder.HandleCommand<HideGroupCommand>()
                .WithHandle<GroupHandle>()
                .WithHandle<GroupActivitiesHandle>();

            builder.HandleCommand<UnhideGroupCommand>()
                .WithHandle<GroupHandle>()
                .WithHandle<GroupActivitiesHandle>();
        }

        private static void ConfigureMediaBindings(BindingBuilder builder)
        {
            builder.HandleCommand<VideoConvertedCommand>()
                .WithHandle<VideoHandler>()
                .WithHandle<EventHandler>()
                .WithHandle<NewsHandler>()
                .WithHandle<SocialHandler>();
        }

    }
}