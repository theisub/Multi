namespace Core
{
    using System;
    using System.Timers;

    /// <summary>
    /// Adapter for for the System.Timers.Timer.
    /// </summary>
    public class SystemTimersTimerAdapter : ITimer
    {
        private Timer innerTimer;
        private Action timerAction;

        public bool IsStarted()
        {
            return innerTimer != null && innerTimer.Enabled;
        }

        public void StartTimer(Action action, TimeSpan interval)
        {
            timerAction = action;
            innerTimer = new Timer(interval.TotalMilliseconds);
            innerTimer.Elapsed += innerTimer_Elapsed;
            innerTimer.Start();
        }

        public void StopTimer()
        {
            if (innerTimer != null)
            {
                innerTimer.Stop();
                innerTimer = null;
            }
        }

        void innerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerAction();
        }
    }
}
