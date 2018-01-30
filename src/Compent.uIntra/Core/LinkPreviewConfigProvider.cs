using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using uIntra.Core;
using uIntra.Core.Utils;

namespace Compent.uIntra.Core
{
    public class LinkPreviewConfigProvider : ILinkPreviewConfigProvider
    {
        private readonly LinkDetectionConfigurationSection _section;

        public LinkPreviewConfigProvider()
        {
            _section = (LinkDetectionConfigurationSection) ConfigurationManager.GetSection(LinkDetectionConfigurationSection.SectionName);
        }

        public FooBananaConfig Config => new FooBananaConfig(_section.Regexes.Select(r => r.Key));
    }

    public class DetectionRegex : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key => (string)base["key"];
    } 

    [ConfigurationCollection(typeof(DetectionRegex))]
    public class DetectionRegexCollection : ConfigurationElementCollection, IEnumerable<DetectionRegex>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DetectionRegex();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DetectionRegex)element).Key;
        }

        IEnumerator<DetectionRegex> IEnumerable<DetectionRegex>.GetEnumerator()
        {
            var baseEnumerator = base.GetEnumerator();
            while (baseEnumerator.MoveNext())
            {
                yield return (DetectionRegex)baseEnumerator.Current;
            }
        }
    }

    public class LinkDetectionConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "linkDetectionConfiguration";

        [ConfigurationProperty("regexes")]
        public DetectionRegexCollection Regexes => this["regexes"] as DetectionRegexCollection;
    }
}