namespace Core
{
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    /// <summary>
    /// A class that either starts an asynchronous operation directly or 
    /// uses timers to start an asynchronous operation indirectly. This is
    /// the initial version which is lacks any testability support.
    /// </summary>
    public class SystemUnderTestInitial
    {
        private Timer timer;
        private readonly Object lockObject = new Object();

        /// <summary>
        /// Start asynchronous operation without any testability support
        /// </summary>
        public void StartAsynchronousOperation()
        {
            // here goes any initialization
            this.Message = "Init";

            Task.Run(() =>
            {
                Thread.Sleep(1500);
                Message += " Work";
            });
        }

        public void StartRecurring()
        {
            this.Message = "Init";

            // Set up timer with 1 sec delay and 1 sec interval.
            this.timer = new Timer(o => { lock(lockObject){ Message += " Poll";} }, null, new TimeSpan(0, 0, 0, 1), new TimeSpan(0, 0, 0, 1));
        }

        public void StopRecurring()
        {
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            this.timer = null;
        }

        public string Message { get; private set; }

    }
}
