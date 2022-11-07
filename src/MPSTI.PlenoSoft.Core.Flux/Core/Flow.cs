using MPSTI.PlenoSoft.Core.Flux.Parameters;
using System;

namespace MPSTI.PlenoSoft.Core.Flux.Core
{
	public class Flow : IFlow
	{
		private readonly FlowArg _flowArg;

		public static IFlow To(out FlowArg flowArg, string description) { return new Flow(description, out flowArg); }
		public static IFlow With(FlowArg flowArg, string description) { return new Flow(description, flowArg); }

		private Flow(string description, out FlowArg flowArg)
		{
			_flowArg = flowArg = new FlowArg();
			_flowArg.AddTrack(description);
		}

		private Flow(string description, FlowArg flowArg)
		{
			_flowArg = flowArg ?? new FlowArg();
			_flowArg.AddTrack(description);
		}

		public IFlow Do(string description, Action<FlowArg> step)
		{
			return Call("Do", description, step);
		}

		public IFlow Validating(string description, Func<FlowArg, bool> test)
		{
			return Call("Validating", "(" + description + ")", flowArg =>
				{
					var result = (test?.Invoke(flowArg)).GetValueOrDefault(false);
					if (!result)
						flowArg.AddValidation("(" + description + ") was violated!");
				});
		}

		private IFlow Call(string prefix, string description, Action<FlowArg> step)
		{
			_flowArg.AddTrack("Start " + prefix + ": " + description);
			CallImpl(step);
			_flowArg.AddTrack("End " + prefix + ": " + description);
			return this;
		}

		private void CallImpl(Action<FlowArg> step)
		{
			if (_flowArg.Status)
			{
				try
				{
					step?.Invoke(_flowArg);
				}
				catch (Exception exception)
				{
					_flowArg.AddException(exception);
				}
			}
		}
	}
}