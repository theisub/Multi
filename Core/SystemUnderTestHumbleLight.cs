namespace Core
{
    using System;
    using System.Threading.Tasks;
    using System.Timers;

    /// <summary>
    /// A class that either starts new threads directly or 
    /// uses timers to to start threads indirectly.
    /// Some formerly private methods are now public to allow
    /// testing without multithreading. 
    /// </summary>
    public class SystemUnderTestHumbleLight
    {
        private Timer timer;
        private bool throwExceptionFlag;


        /// <summary>
        /// Starts asynchronous operation which is implemented in Do1() and Do2().
        /// </summary>
        public void StartAsynchronousOperation()
        {
            Init();

            Task task1 = Task.Run((Action)Do1);
            task1.ContinueWith(Do2Internal, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void StartRecurring()
        {
            Init();

            timer = new Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        public void StopRecurring()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        public string Message { get; private set; }

        public void PrepareToThrowException()
        {
            throwExceptionFlag = true;
        }

        /// <summary>
        /// Changed to public to allow access for tests
        /// </summary>
        public void Init()
        {
            // here goes any initialization
            this.Message = "Init";
        }

        /// <summary>
        /// Changed to public to allow access for tests
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        public void Do1()
        {
            // Sometimes the asynchronous operation throws an exceptions 
            if (throwExceptionFlag){throw new Exception();}

            // here goes the first asynchronous operation
            this.Message += " Work";
        }

        /// <summary>
        /// Changed to public to allow access for tests
        /// </summary>
        /// <param name="task"></param>
        public void Do2(TaskStatus taskStatus)
        {
            // here goes the second asynchronous operation
            if (taskStatus == TaskStatus.RanToCompletion)
            {
                this.Message += " OK";
            }
            if (taskStatus == TaskStatus.Faulted)
            {
                this.Message += " NOK";
            }
        }

        /// <summary>
        /// Changed to public to allow access for tests
        /// </summary>
        public void DoRecurring()
        {
            if (throwExceptionFlag) { throw new Exception(); }

            this.Message += " Poll";
        }


        private void Do2Internal(Task task)
        {
            Do2(task.Status);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.DoRecurring();
        }

    }
}
