using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Controllers
{
    public class EnumController : ApiController
    {
        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> MediaFolderType()
        {
            return Enum.GetValues(typeof(MediaFolderTypeEnum)).OfType<Enum>().Select(x => new KeyValuePair<string, string>(x.ToString(), x.GetDisplayName()));
        }
    }
}