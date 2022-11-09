using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Securities
{
	public class JwtService : IJwtService
	{
		private const string cSecretKey = "2HrfRcCaRTz2ucwYWKgwxF3paUdHjtqhtFwFss4r7pQcybaFYRRgQZM8AcpZhyFK3KxYwyQps85RCssXCtXVdW26";
		private static readonly byte[] SymmetricKey = Convert.FromBase64String(cSecretKey);
		private static readonly SymmetricSecurityKey SymmetricSecurityKey = new(SymmetricKey);
		private static readonly SigningCredentials SigningCredentials = new(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
		private static readonly TokenValidationParameters ValidationParameters = new() { RequireExpirationTime = true, ValidateIssuer = false, ValidateAudience = false, IssuerSigningKey = SymmetricSecurityKey };
		private static readonly JsonWebTokenHandler JsonWebTokenHandler = new();

		private readonly SecurityInfo _securityInfo;
		public JwtService(IServiceProvider serviceProvider)
		{
			_securityInfo = serviceProvider.GetRequiredService<SecurityInfo>();
		}

		public async Task<AccessToken> GenerateTokenAsync(IUserInfo usuario)
		{
			return await Task.Factory.StartNew(() => GenerateToken(usuario));
		}

		private AccessToken GenerateToken(IUserInfo usuario)
		{
			var now = DateTime.UtcNow;
			var claims = GenerateClaims(usuario);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				SigningCredentials = SigningCredentials,
				NotBefore = now,
				IssuedAt = now,
				Expires = now.Add(_securityInfo.TimeOut),
				Issuer = _securityInfo.ApplicationName,
				Audience = _securityInfo.ApplicationName,
				Claims = ToDic(claims),
			};
			var token = JsonWebTokenHandler.CreateToken(tokenDescriptor);

			var accessToken = new AccessToken
			{
				Scheme = IJwtService.AuthenticationType,
				Token = token,
				TimeOut = _securityInfo.TimeOut,
				ExpiresAt = now.Add(_securityInfo.TimeOut),
				Claims = tokenDescriptor.Claims,
			};

			return accessToken;
		}

		private IEnumerable<Claim> GenerateClaims(IUserInfo usuario)
		{
			yield return new Claim(ClaimTypes.Sid, usuario.Id.ToString());
			yield return new Claim(ClaimTypes.Name, usuario.Nome);
			yield return new Claim(ClaimTypes.Email, usuario.EMail);
			yield return new Claim(ClaimTypes.Role, usuario.Roles);
			yield return new Claim(ClaimTypes.MobilePhone, usuario.Celular);
			yield return new Claim(ClaimTypes.UserData, usuario.UserData);
			yield return new Claim(ClaimTypes.Expiration, _securityInfo.ExpiresIn.ToString());
		}

		public AccessToken GetAccessToken(string token)
		{
			if (token.StartsWith(IJwtService.AuthenticationType, StringComparison.OrdinalIgnoreCase))
				token = token[IJwtService.AuthenticationType.Length..].Trim();

			var tokenValidation = JsonWebTokenHandler.ValidateToken(token, ValidationParameters);
			var jsonWebToken = JsonWebTokenHandler.ReadJsonWebToken(token);
			var jwtToken = jsonWebToken?.EncodedToken ?? token;
			var claims = tokenValidation?.ClaimsIdentity?.Claims ?? jsonWebToken.Claims;
			var now = DateTime.UtcNow;

			var accessToken = new AccessToken
			{
				Scheme = IJwtService.AuthenticationType,
				Token = jwtToken,
				IsValid = true, //tokenValidation.IsValid,
				HasExpired = jsonWebToken.ValidFrom > now || now > jsonWebToken.ValidTo,
				Claims = ToDic(claims),
				TimeOut = Get(claims, ClaimTypes.Expiration, v => TimeSpan.FromSeconds(Convert.ToInt64(v))),
				ExpiresAt = Get(claims, "exp", v => DateTime.UnixEpoch.AddSeconds(Convert.ToInt64(v))),
				UserInfo = GetUserInfo(claims),
			};

			return accessToken;
		}

		private IUserInfo GetUserInfo(IEnumerable<Claim> claims)
		{
			return new UserInfo
			{
				Id = Get(claims, ClaimTypes.Sid, v => Convert.ToInt64(v)),
				Nome = Get(claims, ClaimTypes.Name, v => v),
				EMail = Get(claims, ClaimTypes.Email, v => v),
				Roles = Get(claims, ClaimTypes.Role, v => v),
				Celular = Get(claims, ClaimTypes.MobilePhone, v => v),
				UserData = Get(claims, ClaimTypes.Role, v => v),
			};
		}

		private T Get<T>(IEnumerable<Claim> claims, string claimType, Func<string, T> selector)
		{
			var value = claims?.FirstOrDefault(c => c.Type == claimType)?.Value;
			return selector.Invoke(value);
		}

		private Dictionary<string, object> ToDic(IEnumerable<Claim> claims)
		{
			return claims.ToDictionary<Claim, string, object>(c => c.Type, c => c.Value);
		}
	}

	public interface IUserInfo
	{
		long Id { get; set; }
		string Nome { get; set; }
		string EMail { get; set; }
		string Celular { get; set; }
		string Roles { get; set; }
		string UserData { get; }
		bool IsValid { get; }
	}

	public class AccessToken
	{
		public string Scheme { get; set; }
		public string Token { get; set; }
		public long ExpiresIn => Convert.ToInt64(TimeOut.TotalSeconds);
		public DateTime ExpiresAt { get; set; }
		public IDictionary<string, object> Claims { get; set; }


		[JsonIgnore]
		public TimeSpan TimeOut { get; set; }

		[JsonIgnore]
		public bool IsValid { get; set; }

		[JsonIgnore]
		public bool HasExpired { get; set; }

		[JsonIgnore]
		public IUserInfo UserInfo { get; internal set; }
	}

	public class SecurityInfo
	{
		public string ApplicationName { get; set; }
		public TimeSpan TimeOut { get; set; } = TimeSpan.FromDays(1);
		public long ExpiresIn => Convert.ToInt64(TimeOut.TotalSeconds);
	}
}