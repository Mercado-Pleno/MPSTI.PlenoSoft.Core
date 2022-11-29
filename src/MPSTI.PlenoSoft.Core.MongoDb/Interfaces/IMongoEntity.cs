namespace MPSTI.PlenoSoft.Core.MongoDb.Interfaces
{
	public interface IMongoEntity<out TId>
	{
		TId Id { get; }
	}
}