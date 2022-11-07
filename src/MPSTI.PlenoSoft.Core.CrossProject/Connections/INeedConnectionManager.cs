namespace MPSTI.PlenoSoft.Core.CrossProject.Connections
{
	public interface INeedConnectionManager
	{
		IConnectionManager ConnectionManager { get; }

		void SetConnectionManager(IConnectionManager connectionManager);
	}
}