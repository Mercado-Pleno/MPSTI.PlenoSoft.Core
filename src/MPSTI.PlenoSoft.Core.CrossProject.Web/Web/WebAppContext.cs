using Microsoft.AspNetCore.Http;
using System;

namespace MPSTI.PlenoSoft.Core.CrossProject.Web.Web
{
	public class WebAppContext
	{
		private static IHttpContextAccessor _httpContextAccessor;
		private static HttpContext Context => _httpContextAccessor.HttpContext;
		private static HttpRequest Request => Context.Request;
		private static string AppRootUrl => $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
		public static Uri AppRootUri => new Uri(AppRootUrl);


		public static void Configure(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
	}
}