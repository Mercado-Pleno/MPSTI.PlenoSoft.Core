using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.CrossProject.Web.Web
{
	public static class WebAppExtensions
	{
		public static IApplicationBuilder UseWebAppConfig(this IApplicationBuilder app, string requestPathForStaticFiles)
		{
			app.AddGlobalization("pt-BR");
			app.UseSession();
			app.UseCustomStaticFiles(requestPathForStaticFiles);
			app.UseWebAppContext();
			app.UseIFramePermission("AllowAll");
			app.UseIFramePermission("ALLOW-FROM https://example.com/");
			return app;
		}

		public static IServiceCollection AddWebAppContext(this IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options => { options.CheckConsentNeeded = context => true; options.MinimumSameSitePolicy = SameSiteMode.None; });
			services.AddSession(options => { options.IdleTimeout = TimeSpan.FromSeconds(10); options.Cookie.HttpOnly = true; });
			services.AddDistributedMemoryCache();
			services.AddHttpContextAccessor();
			services.AddIFramePermission();
			return services;
		}

		private static IApplicationBuilder AddGlobalization(this IApplicationBuilder app, params string[] supportedCultures)
		{
			var localizationOptions = new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(culture: supportedCultures.First(), uiCulture: supportedCultures.First()),
				SupportedCultures = supportedCultures.Select(ci => new CultureInfo(ci)).ToList(),
				SupportedUICultures = supportedCultures.Select(ci => new CultureInfo(ci)).ToList()
			};

			app.UseRequestLocalization(localizationOptions);
			return app;
		}

		private static IServiceCollection AddHttpContextAccessor(this IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			return services;
		}

		private static IServiceCollection AddIFramePermission(this IServiceCollection services)
		{
			services.AddAntiforgery(o => { o.SuppressXFrameOptionsHeader = true; });
			return services;
		}

		/// <summary>
		/// https://www.niceonecode.com/Question/20462/Refused-to-display-in-a-frame-because-it-set-X-Frame-Options-to-sameorigin
		/// <system.webServer>
		/// 	<httpProtocol>
		/// 		<customHeaders>
		/// 			<add name="Access-Control-Allow-Origin" value="*"/>
		/// 			<add name="X-Frame-Options" value="ALLOW-FROM https://example.com/"/>
		/// 		</customHeaders>
		/// 	</httpProtocol>
		/// <system.webServer>
		/// </summary>
		/// <param name="app"></param>
		/// <param name="permission"></param>
		/// <returns></returns>
		private static IApplicationBuilder UseIFramePermission(this IApplicationBuilder app, StringValues permission)
		{
			async Task useIFramePermission(HttpContext context, Func<Task> next)
			{
				context.Response.Headers.Remove("X-Frame-Options");
				context.Response.Headers.Add("X-Frame-Options", permission);
				await next();
			};

			app.Use(useIFramePermission);
			return app;
		}

		private static IApplicationBuilder UseWebAppContext(this IApplicationBuilder app)
		{
			var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
			WebAppContext.Configure(httpContextAccessor);
			return app;
		}

		private static IApplicationBuilder UseCustomStaticFiles(this IApplicationBuilder app, string requestPath)
		{
			var staticFileOptions = GetStaticFileOptions(requestPath);
			app.UseStaticFiles();
			app.UseStaticFiles(staticFileOptions);
			return app;
		}

		private static StaticFileOptions GetStaticFileOptions(string requestPath)
		{
			return new StaticFileOptions
			{
				RequestPath = requestPath,
				DefaultContentType = "image/jpeg",
				ServeUnknownFileTypes = true,
				HttpsCompression = HttpsCompressionMode.DoNotCompress,
				FileProvider = new PhysicalFileProvider(WebAppConfig._staticFiles),
				//ContentTypeProvider = new FileExtensionContentTypeProvider(),
			};
		}
	}
}