using System;
using System.Threading.Tasks;
using UBaseline.Search.Core;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Core.Search.Indexers
{
    public interface IUserTagIndexer : ISearchDocumentIndexer
    {
        Task Index(UserTag tag);
        Task Delete(Guid id);
    }
}