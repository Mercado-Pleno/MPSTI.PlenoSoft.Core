namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Repository.Interfaces
{
	public interface ICosmosEntity
	{
		string Id { get; }
		string PartitionKeyValue { get; }
	}
}