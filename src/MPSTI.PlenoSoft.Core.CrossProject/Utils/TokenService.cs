using Microsoft.IdentityModel.Tokens;
using MPSTI.PlenoSoft.Core.CrossProject.Apis;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MPSTI.PlenoSoft.Core.CrossProject.Utils
{
	public static class TokenService
	{
		public static JwtAuthorization GenerateToken(Usuario usuario, string secretKey)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var expires = DateTime.UtcNow.AddHours(2);
			var securityTokenDescriptor = GetSecurityTokenDescriptor(secretKey, usuario, expires);
			var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
			var token = tokenHandler.WriteToken(securityToken);

			var jwtAuthorization = new JwtAuthorization
			{
				Expires = expires,
				Scheme = "Bearer",
				Token = token,
				Usuario = usuario,
			};

			return jwtAuthorization;
		}

		private static SecurityTokenDescriptor GetSecurityTokenDescriptor(string secretKey, Usuario user, DateTime expires)
		{
			var key = Encoding.ASCII.GetBytes(secretKey);
			var symmetricSecurityKey = new SymmetricSecurityKey(key);
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Expires = expires,
				NotBefore = DateTime.UtcNow.AddMinutes(-1),
				SigningCredentials = signingCredentials,
				Subject = user.GetClaimsIdentity(),
			};
			return tokenDescriptor;
		}
	}
}