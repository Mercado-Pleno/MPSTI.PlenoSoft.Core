using MPSC.PlenoSoft.ControlFlux.Parameters;
using System;

namespace MPSC.PlenoSoft.ControlFlux.Core
{
	public interface IFlux
	{
		IFlux Do(String description, Action<FluxArg> step);
		IFlux Validating(String description, Func<FluxArg, Boolean> test);
	}
}