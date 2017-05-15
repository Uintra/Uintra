using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace uCommunity.Tagging.Web
{
    public abstract class TagsControllerBase : SurfaceController
    {
        protected virtual string TagsViewPath { get; set; } = "~/App_Plugins/Tagging/Views/TagsView.cshtml";

        protected readonly ITagsService _tagsService;

        protected TagsControllerBase(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        public virtual ActionResult Tags(IEnumerable<string> tags)
        {
            return PartialView(TagsViewPath, tags);
        }

        public virtual JsonResult TagsAutocomplete(string query)
        {
            var result = _tagsService.GetAll().Where(el => el.Text.StartsWith(query, StringComparison.OrdinalIgnoreCase));

            return Json(new { Tags = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
