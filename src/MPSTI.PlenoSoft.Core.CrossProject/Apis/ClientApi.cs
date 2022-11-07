using RestSharp;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.CrossProject.Apis
{
	public sealed class ClientApi : AbstractApi
	{
		private readonly IAuthApi _authApi;

		public ClientApi(Uri baseUrlApi, IAuthApi authApi = null) : base(baseUrlApi) => _authApi = authApi;

		public async Task<RestResponse> Send(RestRequest restRequest)
		{
			_authApi?.TryUpdateTokenInRequest(restRequest);
			return await SendImpl(restRequest);
		}

		public async Task<RestResponse<T>> Send<T>(RestRequest restRequest)
		{
			_authApi?.TryUpdateTokenInRequest(restRequest);
			return await SendImpl<T>(restRequest);
		}
	}
}