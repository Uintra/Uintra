using System;
using System.Web;
using EmailWorker.Data.Infrastructure;
using UBaseline.Core.Domain;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using UBaseline.Core.Settings;
using UBaseline.Shared.Domain;
using UBaseline.Shared.Node;

namespace Uintra20.Infrastructure.Context
{
    public class IntranetRequestContext : UBaselineRequestContext, IIntranetRequestContext
    {

        protected readonly Lazy<IDomainModel> requestedDomain;
        protected readonly Lazy<INodeModel> requestedNode;

        protected Uri RequestedUrl
        {
            get
            {
                var requestedUrlString = HttpContext.Current.Request.QueryString.Get("url");

                if (requestedUrlString.IsNullOrEmpty())
                {
                    return null;
                }

                var requestedUrl = new Uri(requestedUrlString, UriKind.Absolute);

                return requestedUrl;
            }
        }

        public IntranetRequestContext(IDomainModelService domainModelService,
                INodeModelService nodeModelService,
                UBaselineSettings uBaselineSettings)
        : base(domainModelService, nodeModelService, uBaselineSettings)
        {
            this.requestedNode = new Lazy<INodeModel>(new Func<INodeModel>(this.GetRequestedNode));
            this.requestedDomain = new Lazy<IDomainModel>(new Func<IDomainModel>(this.GetRequestedDomain));
        }

        public INodeModel RequestedNode => this.requestedNode.Value;
        public INodeModel RequestedOrReferredNode => this.requestedNode.Value ?? this.node.Value;
        public override INodeModel Node => RequestedOrReferredNode;
        public IDomainModel RequestedDomain => this.requestedDomain.Value;

        protected virtual IDomainModel GetRequestedDomain()
        {
            if (RequestedUrl == null)
            {
                return null;
            }

            return this.domainModelService.GetByHost(RequestedUrl.Host);
        }

        protected virtual INodeModel GetRequestedNode()
        {
            if (this.RequestedDomain == null)
                return (INodeModel)null;
            Uri uri = RequestedUrl;
            if (uri == (Uri)null)
                return (INodeModel)null;
            return this.nodeModelService.Get(uri.AbsolutePath, this.RequestedDomain.RootNodeId);
        }
    }
}