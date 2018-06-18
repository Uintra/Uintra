using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.CommandBus;
using Uintra.Core.Extensions;
using Uintra.Core.User;
using Uintra.Groups.CommandBus;
using Umbraco.Web.WebApi;

namespace Uintra.Groups.Dashboard
{
    public abstract class GroupsSectionControllerBase : UmbracoAuthorizedApiController
    {
        private readonly IGroupService _groupsService;
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly ICommandPublisher _commandPublisher;

        protected GroupsSectionControllerBase(
            IGroupService groupsService,
            IGroupLinkProvider groupLinkProvider,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ICommandPublisher commandPublisher)
        {
            _groupsService = groupsService;
            _groupLinkProvider = groupLinkProvider;
            _intranetUserService = intranetUserService;
            _commandPublisher = commandPublisher;
        }

        public virtual IEnumerable<BackofficeGroupViewModel> GetAll(bool showHidden, string field, Direction direction)
        {
            var groups = showHidden ? _groupsService.GetAll() : _groupsService.GetAllNotHidden();
            var viewModels = groups.Select(s =>
            {
                var viewModel = s.Map<BackofficeGroupViewModel>();
                viewModel.CreatorName = _intranetUserService.Get(s.CreatorId).DisplayedName;
                viewModel.Link = _groupLinkProvider.GetGroupLink(s.Id);
                return viewModel;
            });

            viewModels = Sort(viewModels, field, direction);

            return viewModels;
        }

        [HttpPost]
        public virtual void Hide(Guid groupId, bool hide)
        {
            if (hide)
            {
                var command = new HideGroupCommand(groupId);
                _commandPublisher.Publish(command);
            }
            else
            {
                var command = new UnhideGroupCommand(groupId);
                _commandPublisher.Publish(command);
            }
        }

        protected IEnumerable<BackofficeGroupViewModel> Sort(IEnumerable<BackofficeGroupViewModel> collection, string field, Direction direction)
        {
            var keySelector = GetKeySelector(field);
            switch (direction)
            {
                default:
                    return collection.OrderBy(keySelector);
                case Direction.Desc:
                    return collection.OrderByDescending(keySelector);
            }
        }

        public Func<BackofficeGroupViewModel, object> GetKeySelector(string field)
        {
            switch (field)
            {
                default:
                    return model => model.Title;
                case "createDate":
                    return model => model.CreateDate;
                case "creator":
                    return model => model.CreatorName;
                case "updateDate":
                    return model => model.UpdateDate;
            }
        }
    }
}