using System.Collections.Generic;
using System.Configuration;

namespace Compent.uIntra.Core.LinkPreview.Config
{
    public class LinkDetectionConfigurationSection : ConfigurationSection
    {
        private const string SectionName = "linkDetectionConfiguration";

        public static LinkDetectionConfigurationSection Configuration =>
            (LinkDetectionConfigurationSection) ConfigurationManager.GetSection(SectionName);

        [ConfigurationProperty("regexes")]
        public DetectionRegexCollection Regexes => this["regexes"] as DetectionRegexCollection;


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
                    yield return (DetectionRegex)baseEnumerator.Current;
            }
        }
    }
}