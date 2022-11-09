using Microsoft.AspNetCore.Http;
using MPSTI.PlenoSoft.Core.Azure.Functions.Securities;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Controllers
{
	public abstract class AuthController : AbstractController
	{
		private IUserInfo _currentUser;
		protected IJwtService JwtService => GetService<IJwtService>();

		protected AuthController(IServiceProvider serviceProvider) : base(serviceProvider) { }

		protected override async Task OnExecutingAsync(HttpContext httpContext, CancellationToken cancellationToken) => _currentUser = await GetCurrentUser(httpContext.Request);

		[DebuggerNonUserCode]
		protected async Task<IUserInfo> GetCurrentUser() => _currentUser ??= await GetCurrentUser(HttpContext.Request);

		private async Task<IUserInfo> GetCurrentUser(HttpRequest request)
		{
			var httpStatusCode = HttpStatusCode.Unauthorized;

			try
			{
				if (request == null || !request.Headers.TryGetValue(IJwtService.AuthenticationHeaderName, out var authorizationHeader) || string.IsNullOrWhiteSpace(authorizationHeader))
					throw new AuthenticationException("No Authorization header was present");

				var accessToken = JwtService.GetAccessToken(authorizationHeader);
				if (!accessToken.IsValid)
					throw new AuthenticationException("No Valid Token was present");

				if (accessToken.HasExpired)
					throw new AuthenticationException($"Token Expired: {accessToken.ExpiresAt}");

				if (!accessToken.UserInfo.IsValid())
					throw new AuthenticationException("No identity key was found in the claims.");

				httpStatusCode = HttpStatusCode.Accepted;
				return accessToken.UserInfo;
			}
			finally
			{
				var statusCode = Convert.ToInt32(httpStatusCode);
				request.HttpContext.Response.Headers.Append("AuthorizationStatus", statusCode.ToString());
				request.HttpContext.Response.StatusCode = statusCode;
				if (httpStatusCode != HttpStatusCode.Accepted)
				{
					await request.HttpContext.Response.Body.FlushAsync();
					request.HttpContext.Response.Body.Close();
				}
			}
		}
	}
}