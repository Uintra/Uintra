using System;
using Uintra.Core.OpenGraph.Models;
using Uintra.Core.OpenGraph.Services;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Web
{
    public abstract class OpenGraphControllerBase : SurfaceController
    {
        private readonly IOpenGraphService _openGraphService;

        public OpenGraphControllerBase(IOpenGraphService openGraphService)
        {
            _openGraphService = openGraphService;
        }

        public virtual OpenGraphObject Render(Guid? id)
        {
            var obj = id.HasValue ? _openGraphService.GetOpenGraphObject(id.Value) :
                _openGraphService.GetOpenGraphObject(CurrentPage);
            return obj;
        }
    }
}
