using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Uintra.Core.BrowserCompatibility
{
    public class BrowserCompatibilityConfigurationSection : ConfigurationSection, IBrowserCompatibilityConfigurationSection
    {
        public static BrowserCompatibilityConfigurationSection Configuration => ConfigurationManager.GetSection("browserCompatibilityConfiguration") as BrowserCompatibilityConfigurationSection;

        [ConfigurationProperty("browsers", IsRequired = true)]
        public BrowsersCollection Browsers => (BrowsersCollection)base["browsers"];
    }


    [ConfigurationCollection(typeof(Browser), AddItemName = "browser")]
    public class BrowsersCollection : ConfigurationElementCollection, IEnumerable<Browser>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Browser();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var configElement = element as Browser;
            if (configElement != null) return configElement.Name;
            return string.Empty;
        }

        IEnumerator<Browser> IEnumerable<Browser>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, Count) select this[i]).GetEnumerator();
        }

        public Browser this[int index] => BaseGet(index) as Browser;
    }

    public class Browser : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => (string)base["name"];
        [ConfigurationProperty("version", IsRequired = true)]
        public string Version => (string)base["version"];
    }
}