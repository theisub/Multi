
namespace Core
{
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    /// <summary>
    /// A class that either starts new threads directly or 
    /// uses timers to to start threads indirectly. 
    /// This version uses events to notify clients
    /// about completion of asynchronous operations.  
    /// </summary>
    public class SystemUnderTestEvent
    {
        private Timer rawTimer;
        private bool throwExceptionFlag;

        public event Action AsynchronousOperationDone;
        public event Action ReccuringOperationDone;

        /// <summary>
        /// Start asynchronous operation with firing event
        /// </summary>
        public void StartAsynchronousOperation()
        {
            this.Message = "Init";

            Task.Run(() =>
            {
                // sometimes the asynchronous operation throws an exceptions 
                if (throwExceptionFlag) { throw new Exception(); }
                Thread.Sleep(1500);
                Message += " Work";
                OnAsynchronousOperationDone();
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
                    OnReccuringOperationDone();
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


        protected virtual void OnAsynchronousOperationDone()
        {
            Action handler = this.AsynchronousOperationDone;
            if (handler != null)
            {
                handler();
            }
        }

        protected virtual void OnReccuringOperationDone()
        {
            Action handler = this.ReccuringOperationDone;
            if (handler != null)
            {
                handler();
            }
        }

    }
}
