using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uintra.Core.OpenGraph.Models;
using Umbraco.Core.Models;

namespace Uintra.Core.OpenGraph.Services
{
    public interface IOpenGraphService
    {
        OpenGraphObject GetOpenGraphObject(IPublishedContent content);
        OpenGraphObject GetOpenGraphObject(Guid activityId);
    }
}
