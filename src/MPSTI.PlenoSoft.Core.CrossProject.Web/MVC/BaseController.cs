using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MPSTI.PlenoSoft.Core.CrossProject.Connections;
using System;

namespace MPSTI.PlenoSoft.Core.CrossProject.Web.MVC
{
	public abstract class BaseController : Controller
	{
		protected readonly IConnectionManager ConnectionManager;

		public BaseController(IConnectionManagerFactory connectionManagerFactory)
		{
			ConnectionManager = connectionManagerFactory.CreateConnectionManager();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			ConnectionManager?.Dispose();
		}

		protected string GetAnonymousId(TimeSpan? expires = null)
		{
			const string key = "System_AnonymousId";
			var id = Guid.NewGuid().ToString();

			if (Request.Cookies[key] == null)
				Response.Cookies.Append(key, id, new CookieOptions { Expires = expires.HasValue ? DateTime.Now.Add(expires.Value) : DateTime.Today.AddYears(1) });

			return Request.Cookies[key] ?? id;
		}
	}
}