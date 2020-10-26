//using Uintra20.Features;
//using Uintra20.Infrastructure.Helpers;

//namespace Uintra20.Infrastructure.Providers
//{
//    public class XPathProvider : IXPathProvider
//    {
//        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;

//        public XPathProvider(IDocumentTypeAliasProvider documentTypeAliasProvider)
//        {
//            _documentTypeAliasProvider = documentTypeAliasProvider;
//        }

//        public string UserTagFolderXPath
//        {
//            get
//            {
//                string dataFolderAlias = _documentTypeAliasProvider.GetDataFolder();
//                string userTagsFolderAlias = _documentTypeAliasProvider.GetUserTagFolder();
//                string userTagAlias = _documentTypeAliasProvider.GetUserTag();

//                return XPathHelper.GetXpath(dataFolderAlias, userTagsFolderAlias, userTagAlias);
//            }
//        }
//    }
//}