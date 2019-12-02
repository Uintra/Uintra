using Compent.CommandBus;
using Uintra20.Core.Commands;
using Uintra20.Core.Member.Commands;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Comments.CommandBus;
using Uintra20.Features.Comments.CommandBus.Commands;
using Uintra20.Features.Media;

namespace Uintra20
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
                /*.WithHandle<MemberHandle>()*/;
            builder.HandleCommand<MembersChanged>()
                /*.WithHandle<MemberHandle>()*/;

            builder.HandleCommand<MentionCommand>()
                /*.WithHandle<MentionHandle>()*/;

            return builder.Build();
        }

        private static void ConfigureLikeBindings(BindingBuilder builder)
        {
            //builder.HandleCommand<AddLikeCommand>()
            //    .WithHandle<LikeHandle>()
            //    .WithHandle<LikeNotificationHandle>();

            //builder.HandleCommand<RemoveLikeCommand>()
            //    .WithHandle<LikeHandle>();
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
            //builder.HandleCommand<HideGroupCommand>()
            //    .WithHandle<GroupHandle>()
            //    .WithHandle<GroupActivitiesHandle>();

            //builder.HandleCommand<UnhideGroupCommand>()
            //    .WithHandle<GroupHandle>()
            //    .WithHandle<GroupActivitiesHandle>();
        }

        private static void ConfigureMediaBindings(BindingBuilder builder)
        {
            builder.HandleCommand<VideoConvertedCommand>()
                .WithHandle<MediaHelper>()
                //.WithHandle<EventsService>()
                //.WithHandle<NewsService>()
                .WithHandle<BulletinsService<Bulletin>>();
        }

    }
}