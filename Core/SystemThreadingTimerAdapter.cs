namespace Core
{
    using System;
    using System.Threading;

    /// <summary>
    /// Adapter for for the System.Threading.Timer.
    /// </summary>
    public class SystemThreadingTimerAdapter : ITimer
    {
        private Timer innerTimer;
        private Action timerAction;

        public bool IsStarted()
        {
            return innerTimer != null;
        }

        public void StartTimer(Action action, TimeSpan interval)
        {
            timerAction = action;
            innerTimer = new Timer(TimerCallback,null,interval,interval);
        }

        public void StopTimer()
        {
            innerTimer.Change(Timeout.Infinite, Timeout.Infinite);
            innerTimer = null;
        }

        private void TimerCallback(object state)
        {
            timerAction();
        }
    }
}
