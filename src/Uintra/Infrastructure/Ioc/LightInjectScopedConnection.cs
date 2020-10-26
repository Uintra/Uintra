﻿using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR.Hubs;
using Uintra.Infrastructure.Extensions;
using Umbraco.Core.Composing;

namespace Uintra.Infrastructure.Ioc
{
	public class LightInjectScopedConnection : HubDispatcher
	{
		public LightInjectScopedConnection(HubConfiguration config)
			: base(config)
		{
		}

		protected override Task OnReceived(IRequest request, string connectionId, string data) =>
			Current.Factory.EnsureScopeAsync(container => base.OnReceived(request, connectionId, data));

		public override Task ProcessRequest(HostContext context) =>
			Current.Factory.EnsureScopeAsync(container => base.ProcessRequest(context));
	}
}