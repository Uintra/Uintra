using System;
using System.Linq;
using System.Xml;
using Umbraco.Core;

namespace Uintra.Installer.Extensions
{
    public static class XmlNodeExtensions
    {
        public static string GetAttributeValueFromNode(this XmlNode node, string attributeName)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            string str = string.Empty;
            if (node.Attributes != null && node.Attributes[attributeName] != null)
                str = node.Attributes[attributeName].InnerText;
            return str;
        }

        public static string GetAttributeValue(this XmlNode node, string attributeName)
        {
            if (node == null || attributeName.IsNullOrWhiteSpace())
                return null;
            string str = null;
            if (node.Attributes[attributeName] != null)
                str = node.Attributes[attributeName].Value;
            return str;
        }


        public static bool GetAttributeFlagFromNode(this XmlNode node, string attributeName)
        {
            bool result;
            bool.TryParse(node.GetAttributeValueFromNode(attributeName), out result);
            return result;
        }

        public static T[] ProjectValueFromChildren<T>(this XmlNode node, string childName, Func<XmlNode, T> childFactory)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (childName == null)
                throw new ArgumentNullException(nameof(childName));
            return node.SelectNodes(childName).Cast<XmlNode>().Select<XmlNode, T>(childFactory).ToArray<T>();
        }
    }
}
