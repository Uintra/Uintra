using uIntra.Core;
using uIntra.Tagging.UserTags;

namespace Compent.uIntra.Core
{
    public class XPathProvider : IXPathProvider
    {
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

        public XPathProvider(IDocumentTypeAliasProvider documentTypeAliasProvider)
        {
            _documentTypeAliasProvider = documentTypeAliasProvider;
        }

        public string UserTagFolderXPath
        {
            get
            {
                string dataFolderAlias = _documentTypeAliasProvider.GetDataFolder();
                string userTagsFolder = _documentTypeAliasProvider.GetUserTagFolder();

                return XPathHelper.GetXpath(dataFolderAlias, userTagsFolder);
            }
        }
    }
}