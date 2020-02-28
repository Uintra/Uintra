using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using Uintra20.Core.Jobs.Enums;

namespace Uintra20.Core.Jobs.Configuration
{
    public class JobSettingsConfiguration : ConfigurationSection, IJobSettingsConfiguration
    {
        public static JobSettingsConfiguration Configure =>
            ConfigurationManager.GetSection("UintraJobs") as JobSettingsConfiguration;

        [ConfigurationProperty("settings", IsRequired = true)]
        public JobSettingsCollection Settings => (JobSettingsCollection)base["settings"];
    }

    [ConfigurationCollection(typeof(JobSettings), AddItemName = "add")]
    public class JobSettingsCollection : ConfigurationElementCollection, IEnumerable<JobSettings>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var configElement = element as JobSettings;
            return configElement?.Job;
        }

        IEnumerator<JobSettings> IEnumerable<JobSettings>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, Count) select this[i]).GetEnumerator();
        }

        public JobSettings this[int index] => BaseGet(index) as JobSettings;
    }

    public class JobSettings : ConfigurationElement
    {
        [ConfigurationProperty("job", IsRequired = true, IsKey = true)]
        public string Job => (string)base["job"];

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled => (bool)base["enabled"];

        [ConfigurationProperty("runType", IsRequired = true)]
        public JobRunTypeEnum RunType => (JobRunTypeEnum)Enum.Parse(typeof(JobRunTypeEnum), base["runType"].ToString());

        [ConfigurationProperty("timeType", IsRequired = false)]
        public JobTimeTypeEnum TimeType => (JobTimeTypeEnum)Enum.Parse(typeof(JobTimeTypeEnum), base["timeType"].ToString());

        [ConfigurationProperty("time", IsRequired = false)]
        public int? Time => GetValue<int?>(base["time"]);

        [ConfigurationProperty("atHour", IsRequired = false)]
        public int? AtHour => GetValue<int?>(base["atHour"]);

        [ConfigurationProperty("atMinutes", IsRequired = false)]
        public int? AtMinutes => GetValue<int?>(base["atMinutes"]);

        [ConfigurationProperty("date", IsRequired = false)]
        public DateTime? Date => GetValue<DateTime?>(base["date"]);

        private T GetValue<T>(object value)
        {
            if (value == null)
            {
                return default;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (converter.CanConvertFrom(value.GetType()))
            {
                return (T)converter.ConvertFrom(value);
            }

            return default;
        }
    }


}
