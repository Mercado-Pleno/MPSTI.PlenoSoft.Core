using MPSTI.PlenoSoft.Core.Flux.Parameters;
using System;

namespace MPSTI.PlenoSoft.Core.Flux.Core
{
	public interface IFlow
	{
		IFlow Do(string description, Action<FlowArg> step);
		IFlow Validating(string description, Func<FlowArg, bool> test);
	}
}