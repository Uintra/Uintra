using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface IFeedContentHelper
    {
        IIntranetType GetCreateActivityType(IPublishedContent content);
    }
}
