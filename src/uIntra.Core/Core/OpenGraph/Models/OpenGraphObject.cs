using System;
using System.Text;
using System.Web;
using Umbraco.Core;

namespace Uintra.Core.OpenGraph.Models
{
    public class OpenGraphObject : IHtmlString
    {
        private StringBuilder _builder;

        public OpenGraphObject()
        {
            _builder = new StringBuilder();
        }

        public string Title { get; set; }
        public string SiteName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public int? MediaId { get; set; }

        public override string ToString()
        {
            return ToHtmlString();
        }

        public string ToHtmlString()
        {
            if (!Title.IsNullOrWhiteSpace())
            {
                _builder.Append($"<meta property=\"og:title\" content=\"{Title}\" />");
                _builder.Append(Environment.NewLine);
            }

            if (!SiteName.IsNullOrWhiteSpace())
            {
                _builder.Append($"<meta property=\"og:site_name\" content=\"{SiteName}\" />");
                _builder.Append(Environment.NewLine);
            }

            if (!Type.IsNullOrWhiteSpace())
            {
                _builder.Append($"<meta property=\"og:type\" content=\"{Type}\" />");
                _builder.Append(Environment.NewLine);
            }

            if (!Description.IsNullOrWhiteSpace())
            {
                _builder.Append($"<meta property=\"og:description\" content=\"{Description}\" />");
                _builder.Append(Environment.NewLine);
            }

            if (!Url.IsNullOrWhiteSpace())
            {
                _builder.Append($"<meta property=\"og:url\" content=\"{Url}\" />");
                _builder.Append(Environment.NewLine);
            }

            if (!Image.IsNullOrWhiteSpace())
            {
                _builder.Append($"<meta property=\"og:image\" content=\"{Image}\" />");
                _builder.Append(Environment.NewLine);
            }

            return _builder.ToString();
        }
    }
}
