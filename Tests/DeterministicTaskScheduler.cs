using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    using System;

    /// <summary>
    /// A TaskScheduler to be used for unit testing. 
    /// The class allows to execute new scheduled tasks 
    /// on the same thread as a unit test.
    /// </summary>
    public class DeterministicTaskScheduler : TaskScheduler
    {
        private readonly List<Task> scheduledTasks = new List<Task>(); 

        #region TaskScheduler methods
        protected override void QueueTask(Task task)
        {
            scheduledTasks.Add(task);
        }
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        { 
            scheduledTasks.Add(task);
            return false;
        }
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return scheduledTasks;
        }
        public override int MaximumConcurrencyLevel { get { return 1; } }
        #endregion

        /// <summary>
        /// Runs only the currently scheduled tasks.
        /// </summary>
        public void RunPendingTasks()
        {
            foreach (var task in scheduledTasks.ToArray())
            {
                TryExecuteTask(task);
                
                // Propagate exceptions
                try
                {
                    task.Wait();
                }
                catch (AggregateException aggregateException)
                {
                    throw aggregateException.InnerException;
                }
                finally
                {
                    scheduledTasks.Remove(task);   
                }
            }
        }

        /// <summary>
        /// Runs all tasks until no more scheduled tasks are left.
        /// If a pending task schedules an additional task it will also be executed.
        /// </summary>
        public void RunTasksUntilIdle()
        {
            while (scheduledTasks.Any())
            {
                this.RunPendingTasks();
            }
        }

    }
}
