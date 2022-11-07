using System;

namespace MPSTI.PlenoSoft.Core.CrossProject.Apis
{
	public class JwtAuthorization
	{
		public string Scheme { get; set; }
		public string Token { get; set; }
		public DateTime Expires { get; set; }
		public Usuario Usuario { get; set; }
	}
}