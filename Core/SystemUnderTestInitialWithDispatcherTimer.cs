namespace Core
{
    using System;
    using System.Windows.Threading;

    /// <summary>
    /// A class that uses a DispatcherTimer (WPF) to start an asynchronous operation indirectly. This is
    /// the initial version which is lacks any testability support.
    /// </summary>
    public class SystemUnderTestInitialWithDispatcherTimer
    {
        private DispatcherTimer timer;
        private readonly Object lockObject = new Object();

        public void StartRecurring()
        {
            this.Message = "Init";

            // Set up timer with 1 sec interval.
            this.timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (o, e) =>
            {
                lock (lockObject)
                {
                    Message += " Poll";
                }
            };
            timer.Start();
        }

        public void StopRecurring()
        {
            if (timer != null)
            {
                if (timer.IsEnabled)
                {
                    timer.Stop();
                }
                timer = null;
            }
        }

        public string Message { get; private set; }

    }
}
