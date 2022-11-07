using MPSC.PlenoSoft.ControlFlux.Parameters;
using System;

namespace MPSC.PlenoSoft.ControlFlux.Core
{
	public class Flux : IFlux
	{
		private readonly FluxArg FluxArg;

		public static IFlux To(out FluxArg fluxArg, String description) { return new Flux(description, out fluxArg); }
		public static IFlux With(FluxArg fluxArg, String description) { return new Flux(description, fluxArg); }

		private Flux(String description, out FluxArg fluxArg)
		{
			FluxArg = fluxArg = new FluxArg();
			FluxArg.AddTrack(description);
		}

		private Flux(String description, FluxArg fluxArg)
		{
			FluxArg = fluxArg ?? new FluxArg();
			FluxArg.AddTrack(description);
		}

		public IFlux Do(String description, Action<FluxArg> step)
		{
			return Call("Do", description, step);
		}

		public IFlux Validating(String description, Func<FluxArg, Boolean> test)
		{
			return Call("Validating", "(" + description + ")", fluxArg =>
				{
					var result = (test?.Invoke(fluxArg)).GetValueOrDefault(false);
					if (!result)
						fluxArg.AddValidation("(" + description + ") was violated!");
				});
		}

		private IFlux Call(String prefix, String description, Action<FluxArg> step)
		{
			FluxArg.AddTrack("Start " + prefix + ": " + description);
			CallImpl(step);
			FluxArg.AddTrack("End " + prefix + ": " + description);
			return this;
		}

		private void CallImpl(Action<FluxArg> step)
		{
			if (FluxArg.Status)
			{
				try
				{
					step?.Invoke(FluxArg);
				}
				catch (Exception exception)
				{
					FluxArg.AddException(exception);
				}
			}
		}
	}
}