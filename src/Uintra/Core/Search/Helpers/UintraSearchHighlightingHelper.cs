using Compent.Shared.Search.Elasticsearch.SearchHighlighting;
using Nest;
using Newtonsoft.Json.Linq;

namespace Uintra.Core.Search.Helpers
{
    public class UintraSearchHighlightingHelper : SearchHighlightingHelper
    {
        public override string HighlightPreTag => "<em style='background:#ffffc0'>";
        public override string HighlightPostTag => "</em>";
        public override ISearchResponse<JObject> HighlightResponse(ISearchResponse<JObject> response)
        {
            var baseResult =  base.HighlightResponse(response);
            return baseResult;

            // TODO: Search. Add highlights
            //var highlights = response.Hits.ToDictionary(x => x.Id, x => x.Highlights).ToList();

            //foreach (var document in response.Documents)
            //{
            //    var highlight = highlights.Find(el => el.Key == document.id.ToString());
            //    if (highlight.Key == null)
            //    {
            //        continue;
            //    }

            //    Highlight(document, highlight.Value);
            //}
        }
        //protected virtual void Highlight(dynamic document, Dictionary<string, HighlightHit> fields)
        //{
        //    var panelContent = new List<string>();

        //    foreach (var field in fields.Values)
        //    {
        //        var highlightedField = field.Highlights.FirstOrDefault();
        //        switch (field.Field)
        //        {
        //            case "title":
        //                document.title = highlightedField;
        //                break;
        //            case "description":
        //                document.description = highlightedField;
        //                break;

        //            case "panelContent":
        //                panelContent.AddRange(field.Highlights);
        //                break;

        //            case "panelTitle":
        //                panelContent.AddRange(field.Highlights);
        //                break;

        //            case "attachment.content":
        //                document.attachment.content = highlightedField;
        //                break;
        //        }
        //    }

        //    HighlightAdditional(document, fields, panelContent);

        //    if (panelContent.Any())
        //    {
        //        document.panelContent = panelContent.ToDynamic();
        //    }
        //}

        //protected  void HighlightAdditional(dynamic document, Dictionary<string, HighlightHit> fields, List<string> panelContent)
        //{
        //    foreach (var field in fields.Values)
        //    {
        //        var highlightedField = field.Highlights.FirstOrDefault();
        //        switch (field.Field)
        //        {
        //            case "title":
        //                document.title = highlightedField;
        //                break;
        //            case "fullName":
        //                document.fullName = highlightedField;
        //                break;
        //            case "userTagNames":
        //                document.tagsHighlighted = true;
        //                document.userTagNames = new List<string>() { highlightedField }.ToDynamic();
        //                break;
        //            case "email":
        //                document.email = highlightedField;
        //                break;
        //        }
        //    }
        //}

    }
}