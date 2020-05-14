using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Uintra20.Infrastructure.Utils;

namespace Uintra20.Core.Configuration
{
    public class ConfigurationProvider<TConfiguration> : IConfigurationProvider<TConfiguration>
    {
        private readonly string _settingsFilePath;
        private bool _initialized;
        private readonly object _syncRoot;

        protected TConfiguration Settings;
        protected IEmbeddedResourceService embeddedResourceService;

        public ConfigurationProvider(
            string settingsFilePath)
        {
            _settingsFilePath = settingsFilePath;
            _syncRoot = new object();
            embeddedResourceService = DependencyResolver.Current.GetService<IEmbeddedResourceService>();
        }

        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            lock (_syncRoot)
            {
                if (_initialized)
                {
                    return;
                }

                LoadSettings();
                _initialized = true;
                AssertConfigurationIsValid();
            }
        }

        public void Reinitialize()
        {
            if (!_initialized)
            {
                throw new ApplicationException("Not initialized");
            }
            _initialized = false;
            Initialize();
        }

        public TConfiguration GetSettings()
        {
            if (!_initialized)
            {
                throw new ApplicationException("Not initialized");
            }
            return Settings;
        }

        protected virtual void AssertConfigurationIsValid()
        {

        }

        private void LoadSettings()
        {
            AssertFilePathIsValid();
            //AssertFileExists();
            var content = embeddedResourceService.ReadResourceContent(_settingsFilePath);
            Settings = JsonConvert.DeserializeObject<TConfiguration>(content);
        }

        private void AssertFilePathIsValid()
        {
            if (string.IsNullOrEmpty(_settingsFilePath))
            {
                throw new ApplicationException("SettingsFilePath is empty");
            }
            if (Path.GetExtension(_settingsFilePath) != ".json")
            {
                throw new ApplicationException(_settingsFilePath + " is not .json");
            }
        }

        private void AssertFileExists()
        {
            if (!File.Exists(_settingsFilePath))
            {
                throw new ApplicationException(_settingsFilePath + " file does not exist");
            }
        }

    }
}