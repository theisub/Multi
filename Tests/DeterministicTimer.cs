using System;

namespace Tests
{
    using Core;

    /// <summary>
    /// A timer to be used during unit testing. It allows to start timer operations
    /// on the same thread as the unit test.
    /// </summary>
    public class DeterministicTimer : ITimer
    {
        private Action timerAction;
        private TimeSpan timerInterval;

        private bool isStarted;
        private TimeSpan elapsedTime;

        #region ITimer methods
        public void StartTimer(Action action, TimeSpan interval)
        {
            isStarted = true;
            timerAction = action;
            timerInterval = interval;
            elapsedTime = new TimeSpan();
        }

        public bool IsStarted()
        {
            return isStarted;
        }

        public void StopTimer()
        {
            isStarted = false;
        }
        #endregion

        /// <summary>
        /// Tell the timer that some seconds have elapsed and let the timer execute the timer action. 
        /// </summary>
        /// <param name="seconds"></param>
        public void ElapseSeconds(int seconds)
        {
            TimeSpan newElapsedTime = elapsedTime + new TimeSpan(0, 0, 0, seconds);
            if (isStarted)
            {
                long executionCountBefore = elapsedTime.Ticks / timerInterval.Ticks;
                long executionCountAfter = newElapsedTime.Ticks / timerInterval.Ticks;

                for (int i = 0; i < executionCountAfter - executionCountBefore; i++)
                {
                    timerAction();
                }
            }
            elapsedTime = newElapsedTime;
        }
    }
}
