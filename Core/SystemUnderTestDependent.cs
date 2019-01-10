namespace Core
{
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    /// <summary>
    /// A class that either starts new threads directly or 
    /// uses timers to to start threads indirectly.
    /// This version calls a dependetn object when
    /// asynchronous operations are completed. 
    /// </summary>
    public class SystemUnderTestDependent
    {
        private Timer rawTimer;
        private readonly ISomeInterface dependent;
        private bool throwExceptionFlag;

        public SystemUnderTestDependent(ISomeInterface dependent)
        {
            this.dependent = dependent;
        }

        /// <summary>
        /// Start asynchronous operation with call to dependent object
        /// </summary>
        public void StartAsynchronousOperation()
        {
            this.Message = "Init";

            Task.Run(() =>
            {
                // sometimes the asynchronous operation throws an exceptions 
                if (throwExceptionFlag){throw new Exception();}

                Thread.Sleep(1000);
                Message += " Work";

                // The asynchronous operation is finished
                dependent.DoMore();
            });
        }

        public void StartRecurring()
        {
            // here goes any initialization
            this.Message = "Init";

            rawTimer = new Timer(
                o =>
                {
                    // sometimes the asynchronous operation throws an exceptions 
                    if (throwExceptionFlag) { throw new Exception(); }
                    Message += " Poll";
                    // The asynchronous operation is finished
                    dependent.DoMore();
                }, null, new TimeSpan(0, 0, 0, 1), new TimeSpan(0, 0, 0, 1));
        }

        public void StopRecurring()
        {
            rawTimer.Change(Timeout.Infinite, Timeout.Infinite);
            rawTimer = null;
        }

        public string Message { get; private set; }

        public void PrepareToThrowException()
        {
            throwExceptionFlag = true;
        }
    }
}
