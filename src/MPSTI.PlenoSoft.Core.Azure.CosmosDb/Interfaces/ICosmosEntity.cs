namespace MPSTI.PlenoSoft.Core.Azure.CosmosDb.Interfaces
{
	public interface ICosmosEntity
    {
        string Id { get; }
        string PartitionKeyValue { get; }
    }
}