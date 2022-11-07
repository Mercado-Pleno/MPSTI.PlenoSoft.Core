using System;

namespace MPSTI.PlenoSoft.Core.CrossProject.Connections
{
	public static class ConnectionManagerExtension
	{
		public static TClass CreateInstance<TClass>(this INeedConnectionManager iNeedConnectionManager, Action<TClass> setup = null) where TClass : class, new()
		{
			return iNeedConnectionManager?.ConnectionManager?.CreateInstance(setup);
		}

		public static TClass CreateInstance<TClass>(this INeedConnectionManager iNeedConnectionManager, Action<TClass> setup = null, params object[] parametros) where TClass : class
		{
			return iNeedConnectionManager?.ConnectionManager?.CreateInstance(setup, parametros);
		}

		public static void SetConnectionManager(this INeedConnectionManager iNeedConnectionManager, IConnectionManager connectionManager)
		{
			iNeedConnectionManager?.SetConnectionManager(connectionManager);
		}

		public static void SetConnectionManager(this IConnectionManager connectionManager, INeedConnectionManager iNeedConnectionManager)
		{
			iNeedConnectionManager?.SetConnectionManager(connectionManager);
		}
	}
}