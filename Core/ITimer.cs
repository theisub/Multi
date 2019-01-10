namespace Core
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(TimerContracts))]
    public interface ITimer
    {
        [Pure]
        bool IsStarted();

        void StartTimer(Action action, TimeSpan interval);
               
        void StopTimer();
    }

    [ContractClassFor(typeof(ITimer))]
    public abstract class TimerContracts : ITimer
    {
        public abstract bool IsStarted();

        public void StartTimer(Action action, TimeSpan interval)
        {
            Contract.Requires(!this.IsStarted());
            Contract.Requires(interval>new TimeSpan());
        }

        public void StopTimer()
        {
            Contract.Ensures(!this.IsStarted());
        }
    }
}