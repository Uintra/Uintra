using System.Linq;
using Compent.Shared.Extensions.Bcl;
using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra20.Core.Search.Entities;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Search.Converters.Panel
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