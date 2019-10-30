using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra.Core.OpenGraph.Services;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers
{
    public class OpenGraphController : OpenGraphControllerBase
    {
        public OpenGraphController(IOpenGraphService openGraphService)
            : base (openGraphService)
        {

        }
    }
}