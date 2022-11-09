using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPSTI.PlenoSoft.Core.Azure.Functions.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Controllers
{
	public abstract class AbstractController :
#pragma warning disable CS0618 // Type or member is obsolete
		IFunctionExceptionFilter, IFunctionInvocationFilter
	{
		public async Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken) => await OnExecutedAsync(HttpContext, cancellationToken);
		public async Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken) => await OnExecutingAsync(HttpContext, cancellationToken);
		public async Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken) => await OnExceptionAsync(exceptionContext.Exception, cancellationToken);
#pragma warning restore CS0618 // Type or member is obsolete


		protected readonly IServiceProvider ServiceProvider;
		protected readonly ILogger Logger;
		protected readonly JsonSerializerOptions JsonOptions;

		protected TService GetService<TService>() => ServiceProvider.GetRequiredService<TService>();
		protected IHttpContextAccessor HttpContextAccessor => GetService<IHttpContextAccessor>();
		protected HttpContext HttpContext => HttpContextAccessor.HttpContext;

		public AbstractController(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
			Logger = GetService<ILogger>();
			JsonOptions = GetService<JsonSerializerOptions>();
		}

		protected T GetValueFromHeader<T>(string headerName, HttpRequest httpRequest = null)
		{
			var request = httpRequest ?? HttpContext.Request;
			var headerValue = request.Headers[headerName];
			var value = headerValue.FirstOrDefault() ?? "";
			return (T)Convert.ChangeType(value, typeof(T));
		}

		protected async Task<IEnumerable<Stream>> GetFiles(HttpRequest request)
		{
			if (request.ContentType.StartsWith("application/json"))
			{
				var streamReader = new StreamReader(request.Body);
				var jsonString = await streamReader.ReadToEndAsync();
				var fileUpload = JsonConvert.DeserializeObject<FileUpload>(jsonString);
				var streams = fileUpload.Files.Select(f => f.OpenReadStream());
				return streams.ToArray();
			}

			return Enumerable.Empty<Stream>();
		}

		protected virtual async Task OnExecutedAsync(HttpContext httpContext, CancellationToken cancellationToken) => await Task.CompletedTask;

		protected virtual async Task OnExecutingAsync(HttpContext httpContext, CancellationToken cancellationToken) => await Task.CompletedTask;

		protected virtual async Task OnExceptionAsync(Exception exception, CancellationToken cancellationToken) => await Task.CompletedTask;
	}
}