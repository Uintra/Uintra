﻿using System.Runtime.Caching;
using System.Web.Http;
using Umbraco.Web.Mvc;

namespace Uintra.Infrastructure.Caching.Web
{
	public class CacheController : SurfaceController
	{
		[HttpPost]
		public void Reload()
		{
			MemoryCache.Default.Trim(100);
		}
	}
}