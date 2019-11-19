﻿using Compent.CommandBus;
using Compent.Uintra.Core.Bulletins;
using Compent.Uintra.Core.CommandBus;
using Compent.Uintra.Core.Events;
using Compent.Uintra.Core.News;
using Uintra.Comments.CommandBus;
using Uintra.Core.Media;
using Uintra.Groups.CommandBus;
using Uintra.Likes.CommandBus;
using Uintra.Search;
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
				.WithHandle<MediaHelper>()
				.WithHandle<EventsService>()
				.WithHandle<NewsService>()
				.WithHandle<BulletinsService>();
		}

	}
}