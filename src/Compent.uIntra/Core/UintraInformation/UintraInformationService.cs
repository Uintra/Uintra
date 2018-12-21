using System;
using System.Diagnostics;
using System.Reflection;
using Uintra.Core.ApplicationSettings;

namespace Uintra.Core
{
    public class UintraInformationService : IUintraInformationService
    {
        private readonly IApplicationSettings _applicationSettings;
        public Version Version { get; }
        public Uri DocumentationLink { get; }

        public UintraInformationService(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
            Version = GetVersion();
            DocumentationLink = GetDocumentationLink();
        }

        private Version GetVersion()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            return executingAssembly.GetName().Version;
        }

        private Uri GetDocumentationLink()
        {
            var template = _applicationSettings.UintraDocumentationLinkTemplate;
            var link = string.Format(template, Version.Major, Version.Minor, Version.Build, Version.Revision);
            return new Uri(link);
        }
    }
}
