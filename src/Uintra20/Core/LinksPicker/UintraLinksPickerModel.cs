using System;
using System.Linq;
using System.Web;
using Compent.Extensions;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.LinksPicker
{
    public class UintraLinksPickerModel
    {
        public string Caption { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }

        public UintraLinksPickerViewModel ToViewModel()
        {
            var links = Link.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            string link;

            if (links.Length > 1)
            {
                link = links.FirstOrDefault(x => x.Contains(HttpContext.Current.Request.Url.Authority));

                if (!link.HasValue())
                {
                    link = links.First();
                }
            }
            else
            {
                link = links.FirstOrDefault();
            }

            return new UintraLinksPickerViewModel
            {
                Name = Caption,
                Target = Target,
                Url = link.ToLinkModel()
            };
        }
    }
}