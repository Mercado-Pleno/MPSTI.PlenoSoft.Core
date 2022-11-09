using Newtonsoft.Json;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Securities
{
	public class Login
	{
		public string EMail { get; set; }
		public string Senha { get; set; }
		public string Code2FA { get; set; }

		[JsonIgnore]
		public bool IsValid => !string.IsNullOrWhiteSpace(EMail) && !string.IsNullOrWhiteSpace(Senha);
	}

	public static class LoginExtensions
	{
		public static bool IsValid(this Login login) => login != null && login.IsValid;
	}
}