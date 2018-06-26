using Compent.CommandBus;
using Compent.Uintra.Core.CommandBus;
using Uintra.Comments.CommandBus;
using Uintra.Groups.CommandBus;
using Uintra.Likes.CommandBus;
using Uintra.Users.Commands;

namespace Compent.Uintra
{
    public class CommandBusConfiguration : CommandBindingProviderBase
    {
        protected override BindingConfiguration ConfigureBindings(BindingBuilder builder)
        {
            ConfigureLikeBindings(builder);
            ConfigureCommentBindings(builder);
            ConfigureGroupBindings(builder);

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
    }
}