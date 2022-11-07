using RestSharp;
using System;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.CrossProject.Apis
{
	public static class ParametersCollectionExtension
	{
		public static void RemoveAll(this ParametersCollection parametersCollection, Func<Parameter, bool> filtro = null)
		{
			var parameters = filtro != null ? parametersCollection.Where(filtro) : parametersCollection;
			foreach (var parameter in parameters.ToArray())
				parametersCollection.RemoveParameter(parameter);
		}
	}
}
