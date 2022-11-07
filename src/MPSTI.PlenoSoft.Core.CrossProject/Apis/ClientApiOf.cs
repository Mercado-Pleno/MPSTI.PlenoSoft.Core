using RestSharp;
using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.CrossProject.Apis
{
	public class ClientApi<T>
	{
		public readonly ClientApi _clientApi;
		public ClientApi(Uri uri) => _clientApi = new ClientApi(uri);

		public IEnumerable<T> Get(RestRequest request)
		{
			request.Method = Method.Get;
			var response = _clientApi.Send<IEnumerable<T>>(request);
			return response.Result.Data;
		}

		public T Post(RestRequest request)
		{
			request.Method = Method.Post;
			var response = _clientApi.Send<T>(request);
			return response.Result.Data;
		}

		public T Put(RestRequest request)
		{
			request.Method = Method.Put;
			var response = _clientApi.Send<T>(request);
			return response.Result.Data;
		}

		public T Delete(RestRequest request)
		{
			request.Method = Method.Delete;
			var response = _clientApi.Send<T>(request);
			return response.Result.Data;
		}
	}
}