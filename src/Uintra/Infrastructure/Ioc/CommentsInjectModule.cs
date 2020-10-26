using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.Comments.Helpers;
using Uintra.Features.Comments.Links;
using Uintra.Features.Comments.Services;

namespace Uintra.Infrastructure.Ioc
{
	public class CommentsInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<ICommentLinkPreviewService, CommentLinkPreviewService>();
            services.AddScoped<ICommentLinkHelper, CommentLinkHelper>();
			services.AddScoped<ICommentsHelper, CommentsHelper>();

            return services;
		}
	}
}