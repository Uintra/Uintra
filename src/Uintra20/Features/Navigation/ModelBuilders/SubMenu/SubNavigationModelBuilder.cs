using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Compent.Extensions;
using Uintra20.Core.Configuration;
using Uintra20.Features.Groups.Helpers;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Navigation.Configuration;
using Uintra20.Features.Navigation.Models;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Uintra20.Features.Navigation.ModelBuilders.SubMenu
{
    public class SubNavigationModelBuilder : NavigationModelBuilderBase<SubNavigationMenuModel>, ISubNavigationModelBuilder
    {
        private readonly IGroupHelper _groupHelper;
        private readonly IGroupService _groupService;

        public SubNavigationModelBuilder(
            UmbracoHelper umbracoHelper,
            IConfigurationProvider<NavigationConfiguration> navigationConfigurationProvider,
            IGroupHelper groupHelper,
            IGroupService groupService)
            : base(umbracoHelper, navigationConfigurationProvider)
        {
            _groupHelper = groupHelper;
            _groupService = groupService;
        }

        public override SubNavigationMenuModel GetMenu()
        {
            if (IsHomePage(CurrentPage) || IsShowInHomeNavigation(CurrentPage))
            {
                return null;
            }

            var contentUnderHeading = CurrentPage.AncestorsOrSelf().SingleOrDefault(pc => pc.Parent != null && pc.Parent.IsHeading());

            var subMenuStartPage = contentUnderHeading ?? CurrentPage.AncestorsOrSelf().SingleOrDefault(pc => pc.Parent != null && IsHomePage(pc.Parent));
            if (subMenuStartPage == null)
            {
                return null;
            }

            var model = new SubNavigationMenuModel
            {
                Rows = GetSubNavigationMenuRows(subMenuStartPage, _groupHelper.IsGroupPage(subMenuStartPage)),
                Parent = IsHomePage(CurrentPage.Parent) || IsContentUnavailable(CurrentPage.Parent)
                    ? null
                    : MapToMenuItemModel(CurrentPage.Parent),
                Title = GetNavigationName(subMenuStartPage),
                IsTitleHidden = IsTitleHidden(subMenuStartPage),
                ShowBreadcrumbs = IsShowBreadcrumbs(CurrentPage)
            };


            return model;
        }

        protected override bool IsShowBreadcrumbs(IPublishedContent publishedContent)
        {
            return (publishedContent.IsContentPage() || _groupHelper.IsGroupPage(publishedContent)) && Convert.ToBoolean(ConfigurationManager.AppSettings[NavigationApplicationSettingsConstants.NavigationShowBreadcrumbs]);
        }

        protected override bool IsTitleHidden(IPublishedContent publishedContent)
        {
            return !publishedContent.IsContentPage() && !_groupHelper.IsGroupPage(publishedContent);
        }

        protected override bool IsHideFromNavigation(IPublishedContent publishedContent)
        {
            var result = publishedContent.Value<bool?>(NavigationConfiguration.IsHideFromSubNavigation.Alias);
            return result ?? NavigationConfiguration.IsHideFromSubNavigation.DefaultValue;
        }

        protected virtual IEnumerable<IPublishedContent> GetContentForSubNavigation(IPublishedContent publishedContent)
        {
            var result = (publishedContent.Children.Any() || IsHomePage(publishedContent.Parent))
                ? publishedContent.Children
                : publishedContent.Parent.Children;

            return GetAvailableContent(result);
        }

        protected virtual bool IsHomePage(IPublishedContent content)
        {
            return content.ContentType.Alias == NavigationConfiguration.HomePageAlias;
        }

        protected virtual IEnumerable<SubNavigationMenuRowModel> GetSubNavigationMenuRows(IPublishedContent subMenuStartPage, bool isGroupSubNavigation)
        {
            if (!subMenuStartPage.IsContentPage() && !_groupHelper.IsGroupPage(subMenuStartPage))
            {
                return Enumerable.Empty<SubNavigationMenuRowModel>();
            }

            var activeItems = CurrentPage.AncestorsOrSelf().Where(pc => !pc.IsHeading() && !IsHomePage(pc)).ToList();

            var menuRows = activeItems
                .Select(selectedItem =>
                {
                    var availableContent = GetAvailableContent(selectedItem.Children);
                    if (isGroupSubNavigation)
                    {
                        return ValidateGroupSubNavigationItems(availableContent).Select(MapToSubNavigationMenuItemModel);
                    }
                    return availableContent.Select(MapToSubNavigationMenuItemModel);
                })
                .Select(menuItems => new SubNavigationMenuRowModel
                {
                    Items = menuItems.ToList()
                })
                .ToList();

            menuRows.Reverse();

            var topLevelMenuRow = menuRows.First();
            topLevelMenuRow.Items.Insert(0, MapToSubNavigationMenuItemModel(subMenuStartPage));

            return menuRows;
        }

        protected virtual MenuItemModel MapToMenuItemModel(IPublishedContent publishedContent)
        {
            var isActive = CurrentPage.Path
                .ParseCollection(int.Parse)
                .Contains(publishedContent.Id);

            var result = new MenuItemModel
            {
                Id = publishedContent.Id,
                Name = GetNavigationName(publishedContent),
                Url = publishedContent.Url,
                IsActive = isActive
            };

            return result;
        }

        protected virtual SubNavigationMenuItemModel MapToSubNavigationMenuItemModel(IPublishedContent publishedContent)
        {
            var isActive = CurrentPage.Path
                .ParseCollection(int.Parse)
                .Contains(publishedContent.Id);

            var result = new SubNavigationMenuItemModel
            {
                Id = publishedContent.Id,
                Name = GetNavigationName(publishedContent),
                Url = publishedContent.Url,
                IsActive = isActive
            };

            return result;
        }

        protected virtual IEnumerable<IPublishedContent> ValidateGroupSubNavigationItems(IEnumerable<IPublishedContent> items)
        {
            foreach (var item in items)
            {
                var validatePermission = _groupService.ValidatePermission(item);
                if (validatePermission)
                {
                    yield return item;
                }
            }
        }
    }
}