using System;
using System.Collections.Generic;
using System.Web.Http;
using Uintra20.Features.Media;

namespace Uintra20.Controllers
{
    public class EnumController : ApiController
    {
        [HttpGet]
        public IEnumerable<string> MediaFolderType()
        {
            return Enum.GetNames(typeof(MediaFolderTypeEnum));
        }
    }
}