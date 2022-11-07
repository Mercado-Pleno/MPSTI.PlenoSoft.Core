using RestSharp;
using System;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.CrossProject.Apis
{
	public abstract class AbstractApi
	{
		public readonly Uri _baseUrlApi;
		protected AbstractApi(Uri baseUrlApi) => _baseUrlApi = baseUrlApi;

		protected virtual async Task<RestResponse> SendImpl(RestRequest restRequest)
		{
			var restClient = new RestClient(_baseUrlApi);
			return await restClient.ExecuteAsync(restRequest);
		}

		protected virtual async Task<RestResponse<T>> SendImpl<T>(RestRequest restRequest)
		{
			var restClient = new RestClient(_baseUrlApi);
			return await restClient.ExecuteAsync<T>(restRequest);
		}
	}
}