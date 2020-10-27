using System;
using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Shared.Node;
using Uintra.Features.Navigation.Models;
using Uintra.Infrastructure.Providers;

namespace Uintra.Features.Navigation.Services
{
	public class SubNavigationModelBuilder : ISubNavigationModelBuilder
	{
		private readonly INodeModelService _nodeModelService;
		private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
		private readonly IUBaselineRequestContext _baselineRequestContext;

		public SubNavigationModelBuilder(
			IUBaselineRequestContext baselineRequestContext,
			IDocumentTypeAliasProvider documentTypeAliasProvider,
			INodeModelService nodeModelService)
		{
			_baselineRequestContext = baselineRequestContext;
			_documentTypeAliasProvider = documentTypeAliasProvider;
			_nodeModelService = nodeModelService;
		}

		public SubNavigationMenuItemModel GetMenu()
		{
			if (_baselineRequestContext.Node.ContentTypeAlias == _documentTypeAliasProvider.GetHomePage())
			{
				return null;
			}

			var startItem = GetAncestorOrSelf(IsHeading) ?? GetAncestorOrSelf(IsHomePage);

			if (startItem == null)
			{
				return null;
			}

			var startItemModel = Map(startItem);
			CollectSubItems(startItemModel);
			return startItemModel;
		}

		private INodeModel GetAncestorOrSelf(Func<int, bool> predicate)
		{
			return predicate(_baselineRequestContext.Node.ParentId) ?
				_baselineRequestContext.Node :
				_baselineRequestContext.AncestorNodes.SingleOrDefault(pc => predicate(pc.ParentId));
		}


		private void CollectSubItems(SubNavigationMenuItemModel item)
		{
			item.SubItems = GetChildren(item.Id).Reverse().Select(Map).ToList();

			var activeItem = item.SubItems.SingleOrDefault(i => i.Active);
			if (activeItem == null)
			{
				return;
			}
			if (GetChildren(activeItem.Id).Any())
			{
				CollectSubItems(activeItem);
			}
		}

		private IEnumerable<INodeModel> GetChildren(int id)
		{
			return _nodeModelService.GetDescendants(id).Where(d => d.ParentId == id);
		}

		private SubNavigationMenuItemModel Map(INodeModel nodeModel)
		{

			var result = new SubNavigationMenuItemModel
			{
				Id = nodeModel.Id,
				Name = nodeModel.Name,
				Url = nodeModel.Url,
				Active = IsActive(nodeModel.Id),
				CurrentItem = IsCurrentItem(nodeModel.Id)
			};

			return result;
		}

		private bool IsCurrentItem(int id)
		{
			return _baselineRequestContext.Node.Id == id;
		}

		private bool IsActive(int id)
		{
			return _baselineRequestContext.Node.Id == id || _baselineRequestContext.Node.ParentIds.Contains(id);
		}

		private bool IsHeading(int id)
		{
			var node = _nodeModelService.Get(id);
			return node?.ContentTypeAlias == _documentTypeAliasProvider.GetHeading();
		}


		private bool IsHomePage(int id)
		{
			var node = _nodeModelService.Get(id);
			return node?.ContentTypeAlias == _documentTypeAliasProvider.GetHomePage();
		}
	}
}