using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.Mvc;

namespace uIntra.CentralFeed.Web
{
    public abstract class FeedControllerBase : SurfaceController
    {
        protected abstract string OverviewViewPath { get; }
        protected abstract string ListViewPath { get; }
        protected abstract string NavigationViewPath { get; }
        protected abstract string LatestActivitiesViewPath { get; }

        protected virtual int ItemsPerPage => 8;
    }
}
