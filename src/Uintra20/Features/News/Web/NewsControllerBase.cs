using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Compent.Extensions;
using Compent.Shared.Extensions;
using UBaseline.Shared.NewsPreview;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Feed;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Media;
using Uintra20.Features.News.Models;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;
using Umbraco.Web.WebApi;

namespace Uintra20.Features.News.Web
{
    public abstract class NewsControllerBase : UmbracoApiController
    {
        private readonly INewsService<NewsBase> _newsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IPermissionsService _permissionsService;

        private const PermissionResourceTypeEnum ActivityType = PermissionResourceTypeEnum.News;

        protected NewsControllerBase(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            INewsService<NewsBase> newsService,
            IMediaHelper mediaHelper,
            IActivityTypeProvider activityTypeProvider,
            IActivityLinkService activityLinkService,
            IPermissionsService permissionsService)
        {
            _intranetMemberService = intranetMemberService;
            _newsService = newsService;
            _mediaHelper = mediaHelper;
            _activityTypeProvider = activityTypeProvider;
            _activityLinkService = activityLinkService;
            _permissionsService = permissionsService;
        }

        //public virtual ActionResult Details(Guid id, ActivityFeedOptions options)
        //{
        //    var news = _newsService.Get(id);
        //    var model = GetViewModel(news, options);
        //}

        //public virtual ActionResult Create(IActivityCreateLinks links)
        //{
        //    var model = GetCreateModel(links);
        //}

        [HttpPost]
        public virtual ActionResult Create(NewsCreateModel createModel)
        {
            var newsBaseCreateModel = MapToNews(createModel);
            var activityId = _newsService.Create(newsBaseCreateModel);

            OnNewsCreated(activityId, createModel);

            string redirectUri = _activityLinkService.GetLinks(activityId).Details;
        }

        //public virtual ActionResult Edit(Guid id, ActivityLinks links)
        //{
        //    var news = _newsService.Get(id);
        //    if (news.IsHidden)
        //    {
        //        HttpContext.Response.Redirect(links.Overview);
        //    }

        //    var model = GetEditViewModel(news, links);
        //}

        [HttpPost]
        public virtual HttpResponseMessage Edit(NewsEditModel editModel)
        {
            var cachedActivityMedias = _newsService.Get(editModel.Id).MediaIds;

            var activity = MapToNews(editModel);
            _newsService.Save(activity);

            DeleteMedia(cachedActivityMedias.Except(activity.MediaIds));

            OnNewsEdited(activity, editModel);

            return _newsService.Get(editModel.Id);
        }


        //protected virtual NewsCreateModel GetCreateModel(IActivityCreateLinks links)
        //{
        //    var mediaSettings = _newsService.GetMediaSettings();
        //    var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
        //    var model = new NewsCreateModel
        //    {
        //        PublishDate = DateTime.UtcNow,
        //        OwnerId = currentMemberId,
        //        ActivityType = _activityTypeProvider[ActivityTypeId],
        //        Links = links,
        //        MediaRootId = mediaSettings.MediaRootId,
        //        PinAllowed = IsPinAllowed()
        //    };
        //    return model;
        //}

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

        //protected virtual void FillMediaSettingsData(MediaSettings settings)
        //{
        //    ViewData["AllowedMediaExtensions"] = settings.AllowedMediaExtensions;
        //}

        //protected virtual NewsViewModel GetViewModel(NewsBase news)
        //{
        //    var model = news.Map<NewsViewModel>();
        //    model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
        //    model.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerable();
        //    model.HeaderInfo.Owner = _intranetMemberService.Get(news).Map<MemberViewModel>();
        //    model.CanEdit = _newsService.CanEdit(news);
        //    return model;
        //}

        //protected virtual NewsViewModel GetViewModel(NewsBase news, ActivityFeedOptions options)
        //{
        //    var model = news.Map<NewsViewModel>();

        //    model.CanEdit = _newsService.CanEdit(news);
        //    model.Links = options.Links;
        //    model.IsReadOnly = options.IsReadOnly;

        //    model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
        //    model.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerable();
        //    model.HeaderInfo.Owner = _intranetMemberService.Get(news).Map<MemberViewModel>();
        //    model.HeaderInfo.Links = options.Links;

        //    return model;
        //}

        //protected virtual NewsItemViewModel GetItemViewModel(NewsBase news, IActivityLinks links)
        //{
        //    var model = news.Map<NewsItemViewModel>();
        //    model.MediaIds = news.MediaIds;
        //    model.Expired = _newsService.IsExpired(news);
        //    model.Links = links;

        //    model.HeaderInfo = news.Map<IntranetActivityItemHeaderViewModel>();
        //    model.HeaderInfo.Owner = _intranetMemberService.Get(news).Map<MemberViewModel>();
        //    model.HeaderInfo.Links = links;

        //    model.LightboxGalleryPreviewInfo = new LightboxGalleryPreviewModel
        //    {
        //        MediaIds = news.MediaIds,
        //        DisplayedImagesCount = DisplayedImagesCount,
        //        ActivityId = news.Id,
        //        ActivityType = news.Type
        //    };
        //    return model;
        //}


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

            if (!IsPinAllowed())
            {
                news.IsPinned = false;
                news.EndPinDate = null;
            }

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