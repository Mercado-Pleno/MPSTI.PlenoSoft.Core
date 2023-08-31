using MPSTI.PlenoSoft.Core.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MPSTI.PlenoSoft.Core.Extensions.Utilities
{
    public class ScheduledAction<TKey> : IScheduledAction<TKey> where TKey : notnull
	{
		private bool wasDisposed = false;
		private readonly object _access = new();
		private readonly Dictionary<TKey, Timer> _timers;
		private readonly TimeSpan _dueTime;
		private readonly TimeSpan _period;
		private readonly Action<TKey> _actionOnTimer;

		public ScheduledAction(Action<TKey> actionOnTimer, TimeSpan dueTime, TimeSpan? period = null)
		{
			_timers = new Dictionary<TKey, Timer>();
			_actionOnTimer = actionOnTimer;
			_dueTime = dueTime;
			_period = period ?? Timeout.InfiniteTimeSpan;
		}

		public virtual void Schedule(TKey key)
		{
			lock (_access)
			{
				if (_timers.TryGetValue(key, out var timer))
					timer.Change(_dueTime, _period);
				else
					_timers[key] = new Timer(state => OnTimerCallback(key), key, _dueTime, _period);
			}
		}

		public virtual void Unschedule(TKey key)
		{
			lock (_access)
			{
				if (_timers.TryGetValue(key, out var timer) && _timers.Remove(key))
				{
					timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
					timer.Dispose();
				}
			}
		}

		public virtual void UnscheduleAll()
		{
			lock (_access)
			{
				var keys = _timers.Keys.ToArray();
				foreach (var key in keys)
					Unschedule(key);
			}
		}

		public virtual void ProcessAll()
		{
			lock (_access)
			{
				var keys = _timers.Keys.ToArray();
				foreach (var key in keys)
					OnTimerCallback(key);
			}
		}

		protected virtual void OnTimerCallback(TKey key)
		{
			Unschedule(key);
			_actionOnTimer?.Invoke(key);
		}

		~ScheduledAction() => Dispose(disposing: false);

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			lock (_access)
			{
				if (!wasDisposed)
					ProcessAll();
				wasDisposed = true;
			}
		}
	}
}