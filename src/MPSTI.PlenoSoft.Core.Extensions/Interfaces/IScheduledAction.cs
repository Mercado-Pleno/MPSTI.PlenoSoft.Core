using System;

namespace MPSTI.PlenoSoft.Core.Extensions.Interfaces
{
    public interface IScheduledAction<in TKey> : IDisposable where TKey : notnull
    {
        void Schedule(TKey key);
        void Unschedule(TKey key);
        void UnscheduleAll();
        void ProcessAll();
    }
}