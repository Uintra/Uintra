using System;
using System.Diagnostics;
using System.Reflection;

namespace Uintra.Core
{
    public class UintraInformationService : IUintraInformationService
    {
        public Version Version { get; }
        public Uri DocumentationLink { get; }

        public UintraInformationService()
        {
            Version = GetVersion();
            DocumentationLink = GetDocumentationLink();
        }

        private Version GetVersion()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
            return new Version(fileVersionInfo.ProductVersion);
        }

        private Uri GetDocumentationLink()
        {
            return new Uri("");
        }
    }
}
