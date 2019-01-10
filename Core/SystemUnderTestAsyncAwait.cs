
namespace Core
{
    using System.Threading;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A class that either starts new threads directly or 
    /// uses timers to to start threads indirectly.
    /// This version use the async and await language feature. 
    /// </summary>
    public class SystemUnderTestAsyncAwait
    {
        private Timer timer;
        private readonly Object lockObject = new Object();
        private bool throwExceptionFlag;
        private TimerNotification nextTimerNotification;

        /// <summary>
        /// Start asynchronous operation with usage of async keyword
        /// </summary>
        /// <returns></returns>
        public async Task DoWorkAsync()
        {
            this.Message = "Init";
            await Task.Run(async () =>
            {
                if (throwExceptionFlag) { throw new Exception(); }
                await Task.Delay(1500);
                this.Message += " Work";
            });
        }

        public TimerNotification StartRecurring()
        {
            this.Message = "Init";

            nextTimerNotification = new TimerNotification();

            // Set up timer with 1 sec delay and 1 sec interval.
            this.timer = new Timer(o =>
            {
                lock (lockObject)
                {
                    var currentTimerNotification = nextTimerNotification;
                    nextTimerNotification = new TimerNotification();
                    Message += " Poll";
                    currentTimerNotification.SetNext(nextTimerNotification);
                }
            }, null, new TimeSpan(0, 0, 0, 1), new TimeSpan(0, 0, 0, 1));
            
            return nextTimerNotification;
        }

        public void StopRecurring()
        {
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            this.timer = null;
        }

        public string Message { get; private set; }

        public void PrepareToThrowException()
        {
            throwExceptionFlag = true;
        }
    }
}
