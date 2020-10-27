using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Uintra.Features.Media;
using Uintra.Features.Media.Enums;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Controllers
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