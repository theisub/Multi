namespace Tests
{
    using System;
    using NUnit.Framework;
    using Core;

    public class TestsSolution3Deterministic
    {
        [Test]
        public void TestCodeSynchronously()
        {
            // Setup
            var dts = new DeterministicTaskScheduler();
            var sut = new SystemUnderTestTaskSchedulerTimer(dts);

            // Execute code to schedule first task and return immediately.
            sut.StartAsynchronousOperation();

            // Execute all tasks on the current thread
            dts.RunTasksUntilIdle();
            
            // Assert
            Assert.AreEqual("Init Work1 Work2", sut.Message);
        }

        [Test]
        public void TestCodeSynchronouslyAndExpectException()
        {
            // Setup
            var dts = new DeterministicTaskScheduler();
            var sut = new SystemUnderTestTaskSchedulerTimer(dts);
            sut.PrepareToThrowException();

            // Execute code to create task and return immediately 
            sut.StartAsynchronousOperation();

            // Execute all tasks on the current thread. We expect that an exception is thrown
            Assert.That(dts.RunTasksUntilIdle, Throws.TypeOf<Exception>());
        }

        /// <summary>
        /// VeryFastAndReliableTestWithTimer
        /// </summary>
        [Test]
        public void TestCodeWithTimerSynchronously()
        {
            // Setup
            var dt = new DeterministicTimer();
            var sut = new SystemUnderTestTaskSchedulerTimer(dt);

            // Execute
            sut.StartRecurring();

            // Tell timer that some time has elapsed
            dt.ElapseSeconds(3);
            sut.StopRecurring();

            // Assert that outcome of three executions of the recurring operation is ok.
            Assert.AreEqual("Init Poll Poll Poll", sut.Message);
        }

        [Test]
        public void TestCodeWithTimerSynchronouslyAndExpectException()
        {
            // Setup
            var dt = new DeterministicTimer();
            var sut = new SystemUnderTestTaskSchedulerTimer(dt);
            sut.PrepareToThrowException();

            // Execute
            sut.StartRecurring();

            // Tell timer that some time has elapsed. We expect that an exception is thrown
            Assert.That(()=>dt.ElapseSeconds(3), Throws.TypeOf<Exception>()); 
        }


    }
}
