using System;
using System.Windows.Threading;

namespace WpfApplication
{
    using Core;

    /// <summary>
    /// Adapter for for the DispatcherTimer.
    /// </summary>
    public class DispatcherTimerAdapter : ITimer
    {
        private DispatcherTimer innerTimer;

        private Action timerAction;

        public bool IsStarted()
        {
            return innerTimer != null && innerTimer.IsEnabled;
        }

        public void StartTimer(Action action, TimeSpan interval)
        {
            timerAction = action;
            innerTimer = new DispatcherTimer();
            innerTimer.Tick += this.InnerTimerTick;
            innerTimer.Interval = interval;
            innerTimer.Start();
        }

        public void StopTimer()
        {
            if (innerTimer != null)
            {
                if (innerTimer.IsEnabled)
                {
                    innerTimer.Stop();
                }
                innerTimer = null;
            }
        }

        private void InnerTimerTick(object sender, EventArgs e)
        {
            timerAction();
        }

    }
}
