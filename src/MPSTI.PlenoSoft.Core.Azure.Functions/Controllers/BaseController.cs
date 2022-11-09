using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Controllers
{
	public abstract class BaseController : AbstractController
	{
		public BaseController(IServiceProvider serviceProvider) : base(serviceProvider) { }

		protected override async Task OnExceptionAsync(Exception exception, CancellationToken cancellationToken)
		{
			var httpResponse = HttpContext.Response;
			if (exception.InnerException is AuthenticationException)
				httpResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
			else if (exception is FunctionInvocationException)
				httpResponse.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
			else if (exception is FunctionException)
				httpResponse.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
			else
				httpResponse.StatusCode = (int)HttpStatusCode.InternalServerError;

			var json = JsonConvert.SerializeObject(new { exception.Message });
			httpResponse.Headers.Append("Content-Lenght", json.Length.ToString());
			httpResponse.Headers.Append("Content-Type", "application/json");
			httpResponse.Headers.Append("AuthorizationStatus", httpResponse.StatusCode.ToString());
			await httpResponse.WriteAsync(json, Encoding.UTF8);
		}

		protected FileInfo GetFileInfoFromRoot(params string[] paths)
		{
			var root = GetRootDirectoryInfo();
			return new FileInfo(Path.Combine(root.FullName, Path.Combine(paths)));
		}

		protected DirectoryInfo GetRootDirectoryInfo()
		{
			var assemblyFileInfo = new FileInfo(Assembly.GetCallingAssembly()?.Location);
			return assemblyFileInfo?.Directory?.Parent ?? new DirectoryInfo("./");
		}
	}

	public class FileInfoUtil
	{
		public FileInfo GetFileInfoFromRoot(params string[] paths)
		{
			var root = GetRootDirectoryInfo();
			return new FileInfo(Path.Combine(root.FullName, Path.Combine(paths)));
		}

		public DirectoryInfo GetRootDirectoryInfo()
		{
			var assemblyFileInfo = new FileInfo(Assembly.GetCallingAssembly()?.Location);
			return assemblyFileInfo?.Directory?.Parent ?? new DirectoryInfo("./");
		}
	}
}