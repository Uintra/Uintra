namespace Uintra20.Core
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
                string userTagsFolderAlias = _documentTypeAliasProvider.GetUserTagFolder();
                string userTagAlias = _documentTypeAliasProvider.GetUserTag();

                return XPathHelper.GetXpath(dataFolderAlias, userTagsFolderAlias, userTagAlias);
            }
        }
    }
}