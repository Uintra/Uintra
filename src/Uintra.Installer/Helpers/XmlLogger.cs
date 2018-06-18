using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml.Linq;
using umbraco.interfaces;
using Umbraco.Core.Logging;

namespace Uintra.Installer.Helpers
{
    public static class XmlLogger
    {
        private static string _path;
        private static bool? _isLogFileExists;
        private static XDocument _document;

        private static string Path
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_path))
                    _path = HostingEnvironment.MapPath("~/App_Data/TEMP/Uintra/InstallPackageLog.xml");
                return _path;
            }
        }

        private static bool IsLogFileExists
        {
            get
            {
                if (!_isLogFileExists.HasValue)
                {
                    var file = new FileInfo(Path);
                    _isLogFileExists = file.Exists;
                }
                return _isLogFileExists.Value;
            }
        }

        private static XDocument Document
        {
            get
            {
                if (_document == null)
                {
                    if (IsLogFileExists)
                        _document = XDocument.Load(Path);
                }
                return _document;
            }
        }

        public static void LogCompleteAction<T>(IDictionary<string, string> attributes) where T : IPackageAction
        {
            return;
            //if (!IsLogFileExists)
            //    return;
            //CreateActionElement(typeof(T).Name, true, null, attributes);
            //Document.Save(Path);
        }

        public static void LogFailedAction<T>(string text, Exception e, IDictionary<string, string> attributes) where T : IPackageAction
        {
            LogHelper.Error(typeof(T), text, e);
            if (!IsLogFileExists)
                return;
            MarkInstallationAsFailed();
            var exceptionElement = CreateExceptionElement(text, e);
            CreateActionElement(typeof(T).Name, false, exceptionElement, attributes);
            Document.Save(Path);
        }

        private static void MarkInstallationAsFailed()
        {
            var info = Document.Root.Element("installationInfo");
            info.SetElementValue("status", false);
        }

        private static XElement CreateActionElement(string alias, bool complete, XElement exceptionElement, IDictionary<string, string> attributes)
        {
            var actions = Document.Root.Element("actions");
            var action = new XElement("action");
            foreach (var attribute in attributes)
            {
                action.SetAttributeValue(attribute.Key, attribute.Value);
            }
            action.Add(new XElement("alias", alias));
            action.Add(new XElement("complete", complete));
            if (exceptionElement != null)
                action.Add(exceptionElement);
            actions.Add(action);
            return action;
        }

        private static XElement CreateExceptionElement(string text, Exception e)
        {
            var textElement = new XElement("text", text);
            var errorMessageElement = new XElement("errorMessage", e.Message);
            var stackTraceElement = new XElement("stackTrace");
            stackTraceElement.Add(new XCData(e.StackTrace));
            var exceptionElement = new XElement("exception");
            exceptionElement.Add(textElement);
            exceptionElement.Add(errorMessageElement);
            exceptionElement.Add(stackTraceElement);
            return exceptionElement;
        }
    }
}
