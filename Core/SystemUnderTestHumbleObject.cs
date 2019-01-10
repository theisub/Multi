using System;
using System.Threading.Tasks;

namespace Core
{
    using System.Timers;

    /// <summary>
    /// A class that either starts new threads directly or 
    /// uses timers to to start threads indirectly.
    /// The "HubleObject" refactoring is applied and the functionality
    /// is extracted.
    /// </summary>
    public class SystemUnderTestHumbleObject
    {
        private readonly Functionality functionality;
        private Timer timer;

        public SystemUnderTestHumbleObject()
        {
            this.functionality = new Functionality();
        }

        /// <summary>
        /// Starts asynchronous operation which is implemented in the Functionality class.
        /// </summary>
        public void StartAsynchronousOperation()
        {
            functionality.Init();

            Task task1 = Task.Run((Action)Do1);
            task1.ContinueWith(Do2, 
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void StartRecurring()
        {
            // here goes any initialization
            functionality.Init();

            timer = new Timer(new TimeSpan(0,0,0,1).Milliseconds);
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

        public string Message
        {
            get
            {
                return functionality.Message;
            }
        }

        private void Do1()
        {
            // Call the first asynchronous operation
            functionality.Do1();
        }

        private void Do2(Task task)
        {
            // Call the second asynchronous operation
            functionality.Do2(task.Status);
        }


        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            functionality.DoRecurring();
        }

    }
}
