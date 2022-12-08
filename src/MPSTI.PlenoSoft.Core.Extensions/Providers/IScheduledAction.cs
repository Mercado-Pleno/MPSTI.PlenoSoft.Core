using System;

namespace MPSTI.PlenoSoft.Core.Extensions.Providers
{
	public interface IScheduledAction<in TKey> : IDisposable where TKey : notnull
	{
		void Schedule(TKey key);
		void Unschedule(TKey key);
	}
}