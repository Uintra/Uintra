using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.Node;
using Uintra20.Core.UbaselineModels;
using Uintra20.Features.Navigation.Models;
using Uintra20.Features.Navigation.Services;

namespace Uintra20.Features.Navigation.ModelBuilders.SystemLinks
{
    public class SystemLinksModelBuilder : ISystemLinksModelBuilder
    {
        private readonly ISystemLinksService _systemLinksService;
        private readonly INodeModelService _nodeModelService;

        public SystemLinksModelBuilder(ISystemLinksService systemLinksService, INodeModelService nodeModelService)
        {
            _systemLinksService = systemLinksService;
            _nodeModelService = nodeModelService;
        }

        public IEnumerable<SharedLinkItemViewModel> Get()
        {
            var test = _nodeModelService.AsEnumerable().OfType<SharedLinkItemModel>().ToArray();

            //var result = test.Select(ParseToSystemLinksViewModel)
            //        .OrderBy(x => x.SortOrder);

            return null;
        }

        //private SharedLinkItemViewModel ParseToSystemLinksViewModel(SharedLinkItemModel content)
        //{
        //    var result = new SystemLinksViewModel
        //    {
        //        SortOrder = content.SortOrder,
        //        LinksGroupTitle = content.LinksGroupTitle,
        //        Links = content.Links.Select(x => new SystemLinkItemViewModel
        //        {
        //            Url = x.Link, Name = x.Caption, Target = x.Target
        //        }).ToList()
        //    };
            
        //    return result;
        //}
    }
}