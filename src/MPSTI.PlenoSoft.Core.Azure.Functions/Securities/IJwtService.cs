using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Securities
{
	public interface IJwtService
	{
		const string AuthenticationHeaderName = "Authorization";
		const string AuthenticationType = "Bearer";

		public static string GenerateNewSymmetricSecretKey() => Convert.ToBase64String(new HMACSHA256().Key);

		Task<AccessToken> GenerateTokenAsync(IUserInfo usuario);

		AccessToken GetAccessToken(string authorizationHeader);
	}
}