using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Compent.Shared.Extensions.Bcl;
using UBaseline.Core.Controllers;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Media;
using Uintra20.Features.News.Models;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.Web
{
    public abstract class NewsControllerBase : UBaselineApiController
    {
        private readonly INewsService<NewsBase> _newsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IPermissionsService _permissionsService;

        private const PermissionResourceTypeEnum ActivityType = PermissionResourceTypeEnum.News;

        protected NewsControllerBase(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            INewsService<NewsBase> newsService,
            IMediaHelper mediaHelper,
            IPermissionsService permissionsService)
        {
            _intranetMemberService = intranetMemberService;
            _newsService = newsService;
            _mediaHelper = mediaHelper;
            _permissionsService = permissionsService;
        }

        [HttpPost]
        public virtual NewsViewModel Create(NewsCreateModel createModel)
        {
            var newsBaseCreateModel = MapToNews(createModel);
            var activityId = _newsService.Create(newsBaseCreateModel);

            OnNewsCreated(activityId, createModel);

            return GetViewModel(_newsService.Get(activityId));
        }

        [HttpPost]
        public virtual NewsViewModel Edit(NewsEditModel editModel)
        {
            var cachedActivityMedias = _newsService.Get(editModel.Id).MediaIds;

            var activity = MapToNews(editModel);
            _newsService.Save(activity);

            DeleteMedia(cachedActivityMedias.Except(activity.MediaIds));

            OnNewsEdited(activity, editModel);

            return GetViewModel(_newsService.Get(editModel.Id));
        }

        protected virtual bool IsPinAllowed()
        {
            return _permissionsService.Check(ActivityType, PermissionActionEnum.CanPin);
        }

        protected virtual NewsViewModel GetViewModel(NewsBase news)
        {
            var model = news.Map<NewsViewModel>();
            model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerable();
            model.HeaderInfo.Owner = _intranetMemberService.Get(news).Map<MemberViewModel>();
            model.CanEdit = _newsService.CanEdit(news);
            return model;
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