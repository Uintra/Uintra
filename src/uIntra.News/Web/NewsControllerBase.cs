using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Attributes;
using Uintra.Core.Context;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Extensions;
using Uintra.Core.Feed;
using Uintra.Core.Links;
using Uintra.Core.Media;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Core.User.Permissions.Web;
using static LanguageExt.Prelude;

namespace Uintra.News.Web
{
	[ActivityController(ActivityTypeId)]
	public abstract class NewsControllerBase : ContextController
	{
		protected virtual string ItemViewPath { get; } = "~/App_Plugins/News/List/ItemView.cshtml";
		protected virtual string PreviewItemViewPath { get; } = "~/App_Plugins/News/PreviewItem/PreviewItemView.cshtml";
		protected virtual string DetailsViewPath { get; } = "~/App_Plugins/News/Details/DetailsView.cshtml";
		protected virtual string CreateViewPath { get; } = "~/App_Plugins/News/Create/CreateView.cshtml";
		protected virtual string EditViewPath { get; } = "~/App_Plugins/News/Edit/EditView.cshtml";
		protected virtual int DisplayedImagesCount { get; } = 3;

		private readonly INewsService<NewsBase> _newsService;
		private readonly IMediaHelper _mediaHelper;
		private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
		private readonly IActivityTypeProvider _activityTypeProvider;
		private readonly IActivityLinkService _activityLinkService;
		private readonly IPermissionsService _permissionsService;

		private const int ActivityTypeId = (int)IntranetActivityTypeEnum.News;
		private const PermissionResourceTypeEnum ActivityType = PermissionResourceTypeEnum.News;

		public override Enum ControllerContextType { get; } = ContextType.News;

		protected NewsControllerBase(
			IIntranetMemberService<IIntranetMember> intranetMemberService,
			INewsService<NewsBase> newsService,
			IMediaHelper mediaHelper,
			IActivityTypeProvider activityTypeProvider,
			IActivityLinkService activityLinkService,
			IContextTypeProvider contextTypeProvider,
			IPermissionsService permissionsService) : base(contextTypeProvider)
		{
			_intranetMemberService = intranetMemberService;
			_newsService = newsService;
			_mediaHelper = mediaHelper;
			_activityTypeProvider = activityTypeProvider;
			_activityLinkService = activityLinkService;
			_permissionsService = permissionsService;
		}

		[NotFoundActivity, RestrictedAction(ActivityType, PermissionActionEnum.View)]
		public virtual ActionResult Details(Guid id, ActivityFeedOptions options)
		{
			var news = _newsService.Get(id);
			var model = GetViewModel(news, options);
			AddEntityIdentityForContext(id);
			return PartialView(DetailsViewPath, model);
		}

		[RestrictedAction(ActivityType, PermissionActionEnum.Create)]
		public virtual ActionResult Create(IActivityCreateLinks links)
		{
			var model = GetCreateModel(links);
			return PartialView(CreateViewPath, model);
		}

		[HttpPost]
		[RestrictedAction(ActivityType, PermissionActionEnum.Create)]
		public virtual ActionResult Create(NewsCreateModel createModel)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToCurrentUmbracoPage(Request.QueryString);
			}
			var newsBaseCreateModel = MapToNews(createModel);
			var activityId = _newsService.Create(newsBaseCreateModel);

			OnNewsCreated(activityId, createModel);

			string redirectUri = _activityLinkService.GetLinks(activityId).Details;
			return Redirect(redirectUri);
		}

		[RestrictedAction(ActivityType, PermissionActionEnum.Edit)]
		public virtual ActionResult Edit(Guid id, ActivityLinks links)
		{
			var news = _newsService.Get(id);
			if (news.IsHidden)
			{
				HttpContext.Response.Redirect(links.Overview);
			}

			var model = GetEditViewModel(news, links);
			return PartialView(EditViewPath, model);
		}

		[HttpPost]
		[RestrictedAction(ActivityType, PermissionActionEnum.Edit)]
		public virtual ActionResult Edit(NewsEditModel editModel)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToCurrentUmbracoPage(Request.QueryString);
			}

			var cachedActivityMedias = _newsService.Get(editModel.Id).MediaIds;

			var activity = MapToNews(editModel);
			_newsService.Save(activity);

			DeleteMedia(cachedActivityMedias.Except(activity.MediaIds));

			OnNewsEdited(activity, editModel);

			string redirectUri = editModel.Links.Details;
			return Redirect(redirectUri);
		}


		protected virtual NewsCreateModel GetCreateModel(IActivityCreateLinks links)
		{
			var mediaSettings = _newsService.GetMediaSettings();
			var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
			var model = new NewsCreateModel
			{
				PublishDate = DateTime.UtcNow,
				OwnerId = currentMemberId,
				ActivityType = _activityTypeProvider[ActivityTypeId],
				Links = links,
				MediaRootId = mediaSettings.MediaRootId,
				PinAllowed = IsPinAllowed()
			};
			return model;
		}

		protected virtual NewsEditModel GetEditViewModel(NewsBase news, ActivityLinks links)
		{
			var model = news.Map<NewsEditModel>();
			var mediaSettings = _newsService.GetMediaSettings();
			model.MediaRootId = mediaSettings.MediaRootId;
			model.PinAllowed = IsPinAllowed();
			FillMediaSettingsData(mediaSettings);

			model.Links = links;

			return model;
		}

		protected virtual bool IsPinAllowed()
		{
			return _permissionsService.Check(ActivityType, PermissionActionEnum.CanPin);
		}

		protected virtual void FillMediaSettingsData(MediaSettings settings)
		{
			ViewData["AllowedMediaExtensions"] = settings.AllowedMediaExtensions;
		}

		protected virtual NewsViewModel GetViewModel(NewsBase news)
		{
			var model = news.Map<NewsViewModel>();
			model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
			model.HeaderInfo.Dates = List(news.PublishDate.ToDateTimeFormat());
			model.HeaderInfo.Owner = _intranetMemberService.Get(news).Map<MemberViewModel>();
			model.CanEdit = _newsService.CanEdit(news);
			return model;
		}

		protected virtual NewsViewModel GetViewModel(NewsBase news, ActivityFeedOptions options)
		{
			var model = news.Map<NewsViewModel>();

			model.CanEdit = _newsService.CanEdit(news);
			model.Links = options.Links;
			model.IsReadOnly = options.IsReadOnly;

			model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
			model.HeaderInfo.Dates = List(news.PublishDate.ToDateTimeFormat());
			model.HeaderInfo.Owner = _intranetMemberService.Get(news).Map<MemberViewModel>();
			model.HeaderInfo.Links = options.Links;

			return model;
		}

		protected virtual NewsItemViewModel GetItemViewModel(NewsBase news, IActivityLinks links)
		{
			var model = news.Map<NewsItemViewModel>();
			model.MediaIds = news.MediaIds;
			model.Expired = _newsService.IsExpired(news);
			model.Links = links;

			model.HeaderInfo = news.Map<IntranetActivityItemHeaderViewModel>();
			model.HeaderInfo.Owner = _intranetMemberService.Get(news).Map<MemberViewModel>();
			model.HeaderInfo.Links = links;

			model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
			{
				MediaIds = news.MediaIds,
				DisplayedImagesCount = DisplayedImagesCount,
				ActivityId = news.Id,
				ActivityType = news.Type
			};
			return model;
		}


		protected virtual NewsPreviewViewModel GetPreviewViewModel(NewsBase news, ActivityLinks links)
		{
			var owner = _intranetMemberService.Get(news);
			return new NewsPreviewViewModel
			{
				Id = news.Id,
				Title = news.Title,
				PublishDate = news.PublishDate,
				Owner = owner.Map<MemberViewModel>(),
				ActivityType = news.Type,
				Links = links
			};
		}

		protected virtual NewsBase MapToNews(NewsCreateModel createModel)
		{
			var news = createModel.Map<NewsBase>();
			news.MediaIds = news.MediaIds.Concat(_mediaHelper.CreateMedia(createModel));
			news.PublishDate = createModel.PublishDate.ToUniversalTime().WithCorrectedDaylightSavingTime(createModel.PublishDate);
			news.UnpublishDate = createModel.UnpublishDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(createModel.UnpublishDate.Value);
			news.EndPinDate = createModel.EndPinDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(createModel.EndPinDate.Value);
			news.CreatorId = _intranetMemberService.GetCurrentMemberId();
			news.IsPinned = createModel.PinAllowed && news.IsPinned;
			return news;
		}

		protected virtual NewsBase MapToNews(NewsEditModel editModel)
		{
			var activity = _newsService.Get(editModel.Id);
			activity = Mapper.Map(editModel, activity);
			activity.MediaIds = activity.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));
			activity.PublishDate = editModel.PublishDate.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.PublishDate);
			activity.UnpublishDate = editModel.UnpublishDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.UnpublishDate.Value);
			activity.EndPinDate = editModel.EndPinDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.EndPinDate.Value);
			activity.IsPinned = editModel.PinAllowed && activity.IsPinned;

			return activity;
		}

		protected virtual void DeleteMedia(IEnumerable<int> mediaIds)
		{
			_mediaHelper.DeleteMedia(mediaIds);
		}

		protected virtual void OnNewsCreated(Guid activityId, NewsCreateModel model)
		{
		}

		protected virtual void OnNewsEdited(NewsBase news, NewsEditModel model)
		{
		}
	}
}