namespace MPSTI.PlenoSoft.Exemplo.AzureFunction.Cosmos
{
	public interface ICosmosId
	{
		string Id { get; }
	}

	public interface ICosmosPK
	{
		string PartitionKeyPath { get; }
		string PartitionKeyValue { get; }
	}

	public interface ICosmosConcurrency
	{
		string ETag { get; }
	}

	public interface ICosmosEntity : ICosmosId, ICosmosPK
	{
	}
}