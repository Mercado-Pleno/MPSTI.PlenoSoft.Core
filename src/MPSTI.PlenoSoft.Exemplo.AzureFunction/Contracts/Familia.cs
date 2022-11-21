using MPSTI.PlenoSoft.Core.Azure.CosmosDb.Contracts;
using System;

namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Contracts
{
	public class Familia : CosmosEntityConcurrent<Guid>
	{
		protected override string PartitionKeyPath => "/LastName";
		protected override string PartitionKeyValue => LastName;

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime Updated { get; set; }
		public string Doc { get; set; }

		public Familia() => Updated = DateTime.UtcNow;

	}
}