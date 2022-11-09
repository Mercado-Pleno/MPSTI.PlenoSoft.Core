namespace MPSTI.PlenoSoft.Core.Azure.Functions.Securities
{
	public class UserInfo : IUserInfo
	{
		public long Id { get; set; }
		public string Nome { get; set; }
		public string EMail { get; set; }
		public string Celular { get; set; }
		public string Roles { get; set; }
		public string UserData { get; set; }

		public bool IsValid => !string.IsNullOrWhiteSpace(Nome) && !string.IsNullOrWhiteSpace(EMail) && !string.IsNullOrWhiteSpace(Roles);
	}

	public static class UserInfoExtensions
	{
		public static bool IsValid(this IUserInfo userInfo) => userInfo != null && userInfo.IsValid;
	}
}