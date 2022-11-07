using MPSTI.PlenoSoft.Core.CrossProject.Domains;
using MPSTI.PlenoSoft.Core.CrossProject.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MPSTI.PlenoSoft.Core.CrossProject.Apis
{
	public class Usuario : Entidade
	{
		public string Nome { get; set; }
		public string Email { get; set; }
		public string Celular { get; set; }
		public string Password { get; set; }
		public string Roles { get; set; }

		public ClaimsIdentity GetClaimsIdentity() => new ClaimsIdentity(GetClaims());

		private IEnumerable<Claim> GetClaims()
		{
			Password = Empty;
			yield return new Claim(ClaimTypes.Sid, Id.ToString());
			yield return new Claim(ClaimTypes.Name, Nome);
			yield return new Claim(ClaimTypes.MobilePhone, Celular);
			yield return new Claim(ClaimTypes.Email, Email);
			yield return new Claim(ClaimTypes.Role, Roles);
		}

		public bool VerificarSenha(string password)
		{
			try
			{
				return Crypto.VerificarHash(password, Password);
			}
			finally
			{
				Password = Empty;
			}
		}

		public void CriptografarSenha()
		{
			Password = Crypto.CriarHash(Password);
		}

		public void Validar()
		{
			var rgEmail = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

			// Fazer validações 
			if (string.IsNullOrWhiteSpace(Nome))
				throw new ArgumentException("Parâmetro pode ser vazio", "Nome");

			if (string.IsNullOrWhiteSpace(Password))
				throw new ArgumentException("Parâmetro pode ser vazio", "Senha");

			if (string.IsNullOrWhiteSpace(Roles))
				throw new ArgumentException("Parâmetro pode ser vazio", "Role");

			if (!rgEmail.IsMatch(Email))
				throw new ArgumentException("Parâmetro inválido", "E-Mail");

			if (string.IsNullOrWhiteSpace(Celular))
				throw new ArgumentException("Parâmetro pode ser vazio", "Celular");
		}

		protected override IEnumerable<string> ObterValidacoes()
		{
			throw new NotImplementedException();
		}

		private static readonly string Empty = new string('*', 32);
	}
}