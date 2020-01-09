using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Comments.Helpers;
using Uintra20.Features.Comments.Links;
using Uintra20.Features.Comments.Services;

namespace Uintra20.Infrastructure.Ioc
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