using MPSTI.PlenoSoft.Core.Extensions.Utilities;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.Test.Extensions.Utilities
{
	public class TestingScheduledAction
	{
		[Fact]
		public void AoChamarDisposeDoScheduledAction_DeveDispararTodosOsEventos()
		{
			var lista = new List<string>();
			var timeout = TimeSpan.FromMilliseconds(50);
			var scheduleAction = new ScheduledAction<string>(id => lista.Add(id), timeout);
			lista.Should().HaveCount(0);

			scheduleAction.Schedule("1");
			scheduleAction.Schedule("2");
			scheduleAction.Schedule("3");
			lista.Should().HaveCount(0);

			scheduleAction.Dispose();
			lista.Should().HaveCount(3);

			Thread.Sleep(timeout);
			lista.Should().HaveCount(3);

			Thread.Sleep(timeout);
			lista.Should().HaveCount(3);
		}

		[Fact]
		public void EnquantoEstiverAdicionandoUmEventoAntesDoTimeOut_DeveResetarOTempoENaoDispararEvento()
		{
			var lista = new List<string>();
			var timeout = TimeSpan.FromMilliseconds(100);
			var delay = TimeSpan.FromMilliseconds(10);
			var scheduleAction = new ScheduledAction<string>(id => lista.Add(id), timeout);

			for (int i = 0; i < 15; i++)
			{
				lista.Should().HaveCount(0);
				scheduleAction.Schedule("1");
				scheduleAction.Schedule("2");
				Thread.Sleep(delay);
			}

			lista.Should().HaveCount(0);
			Thread.Sleep(delay);
			Thread.Sleep(timeout);
			lista.Should().HaveCount(2);

			scheduleAction.Dispose();
			lista.Should().HaveCount(2);
		}
	}
}