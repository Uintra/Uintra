using Compent.Shared.Extensions.Bcl;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Features.Search.Converters.Panel
{
    public class UserTagsPanelSearchConverter : SearchDocumentPanelConverter<UserTagsPanelViewModel>
    {
        protected override SearchablePanel OnConvert(UserTagsPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Content = panel.Tags.Value?.JoinWith(" ")
            };
        }
    }
}