using MPSTI.PlenoSoft.Core.MongoDb.Contracts;
using System;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts
{
	public class Pessoa : MongoEntity<string>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime Updated { get; set; }
		public string Doc { get; set; }

		public Pessoa() => Updated = DateTime.UtcNow;
	}
}