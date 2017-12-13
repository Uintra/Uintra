//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using uIntra.Core.Extensions;
//using uIntra.Tagging.Core.Models;
//using Umbraco.Web.Mvc;

//namespace uIntra.Tagging.Web
//{
//    public abstract class TagsControllerBase : SurfaceController
//    {
//        protected virtual string TagsViewPath { get; set; } = "~/App_Plugins/Tagging/Views/TagsView.cshtml";
//        protected virtual string TagsEditViewPath { get; set; } = "~/App_Plugins/Tagging/Views/TagsEditView.cshtml";

//        private readonly ITagsService _tagsService;

//        protected TagsControllerBase(ITagsService tagsService)
//        {
//            _tagsService = tagsService;
//        }

//        public virtual ActionResult Tags(IEnumerable<TagEditModel> tags)
//        {
//            return PartialView(TagsEditViewPath, tags);
//        }

//        public virtual JsonResult Autocomplete(string query)
//        {
//            var result = _tagsService
//                .FindAll(query)
//                .Select(tag => tag)
//                .OrderBy(tag => tag.Text);

//            return Json(new { Tags = result }, JsonRequestBehavior.AllowGet);
//        }

//        public virtual ActionResult ForActivity(Guid activityId)
//        {
//            var tags = _tagsService.GetAllForActivity(activityId).Map<IEnumerable<TagViewModel>>();
//            return PartialView(TagsViewPath, tags);
//        }
//    }
//}