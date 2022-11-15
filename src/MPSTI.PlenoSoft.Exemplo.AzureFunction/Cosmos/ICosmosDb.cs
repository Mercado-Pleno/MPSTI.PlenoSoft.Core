namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public interface ICosmosDb
	{
		string Id { get; }
		string PartitionKeyValue { get; }
	}
}
