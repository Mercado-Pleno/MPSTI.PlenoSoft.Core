using RestSharp;
using System;

namespace MPSTI.PlenoSoft.Core.CrossProject.Apis
{
	public interface IAuthApi
	{
		void TryUpdateTokenInRequest(RestRequest restRequest);
	}

	public class AuthApi : AbstractApi, IAuthApi
	{
		private readonly RestRequest _restRequestGetToken;
		private readonly TimeSpan _sessionTimeOut;
		private JwtAuthorization _jwtAuthorization;
		private DateTimeOffset _lastActivity;

		private string Token => $"{_jwtAuthorization?.Scheme} {_jwtAuthorization?.Token}";
		private bool IsValid => _jwtAuthorization != null && !IsExpired;
		private bool IsExpired => DateTimeOffset.UtcNow > _jwtAuthorization.Expires;
		private bool IsClientIdle => DateTimeOffset.UtcNow - _lastActivity > _sessionTimeOut;
		private void ResetTimerOfTimeOut() => _lastActivity = DateTimeOffset.UtcNow;
		private void ResetParametersOfRequestGetToken() => _restRequestGetToken.Parameters.RemoveAll();

		public AuthApi(Uri baseUrlApi, RestRequest restRequestGetToken)
			: this(baseUrlApi, TimeSpan.FromMinutes(15), restRequestGetToken) { }

		public AuthApi(Uri baseUrlApi, TimeSpan sessionTimeOut, RestRequest restRequestGetToken) : base(baseUrlApi)
		{
			_restRequestGetToken = restRequestGetToken;
			_sessionTimeOut = sessionTimeOut;
			_jwtAuthorization = null;
			ResetTimerOfTimeOut();
		}

		public void TryUpdateTokenInRequest(RestRequest restRequest)
		{
			if (GetValidToken())
				restRequest.AddOrUpdateParameter("Authorization", Token, ParameterType.HttpHeader);
			else
				restRequest.Parameters.RemoveAll(p => p.Name == "Authorization" && p.Type == ParameterType.HttpHeader);
		}

		private bool GetValidToken()
		{
			if (IsClientIdle)
			{
				ResetParametersOfRequestGetToken();
				return false;
			}

			ResetTimerOfTimeOut();

			return IsValid || RenewToken();
		}

		private bool RenewToken()
		{
			try
			{
				_jwtAuthorization = Authenticate();
			}
			catch
			{
				_jwtAuthorization = null;
				throw;
			}
			return IsValid;
		}

		private JwtAuthorization Authenticate()
		{
			return SendImpl<JwtAuthorization>(_restRequestGetToken).Result.Data;
		}
	}
}