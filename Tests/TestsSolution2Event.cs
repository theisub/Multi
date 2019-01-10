namespace Tests
{
    using System;
    using System.Threading;
    using NUnit.Framework;
    using Core;

    [TestFixture]
    public class TestsSolution2Event
    {
        [Test]
        public void SynchronizeTestWithCodeViaEvent()
        {
            // Setup
            var sut = new SystemUnderTestEvent();
            var finishedEvent = new ManualResetEventSlim();
            sut.AsynchronousOperationDone += finishedEvent.Set;

            // Execute
            sut.StartAsynchronousOperation();
            // Wait to be finsihed for maximum 10 sec.
            finishedEvent.Wait(TimeSpan.FromSeconds(10));

            // Assert
            Assert.AreEqual("Init Work", sut.Message);
        }

        [Test]
        public void SynchronizeTestWithCodeViaEventAndExpectException()
        {
            // Setup
            var sut = new SystemUnderTestEvent();
            var finishedEvent = new ManualResetEventSlim();
            sut.AsynchronousOperationDone += finishedEvent.Set;
            sut.PrepareToThrowException();

            // Execute
            sut.StartAsynchronousOperation();
            // Wait to be finsihed for maximum 10 sec.
            finishedEvent.Wait(TimeSpan.FromSeconds(10));

            // Assert
            Assert.AreEqual("Init", sut.Message, "Only Init succeded, because of exception");
        }

        [Test]
        public void SynchronizeTestWithTimerViaEvent()
        {
            // Setup
            var sut = new SystemUnderTestEvent();
            var finishedEvent = new ManualResetEventSlim();
            sut.ReccuringOperationDone += finishedEvent.Set;

            // Execute
            sut.StartRecurring();
            // Wait to be finsihed for maximum 10 sec.
            finishedEvent.Wait(TimeSpan.FromSeconds(10));
            sut.StopRecurring();

            // Assert
            Assert.AreEqual("Init Poll", sut.Message);
        }

        [Test]
        public void SynchronizeTestWithTimerViaEventAndExpectException()
        {
            // Setup
            var sut = new SystemUnderTestEvent();
            var finishedEvent = new ManualResetEventSlim();
            sut.ReccuringOperationDone += finishedEvent.Set;
            sut.PrepareToThrowException();

            // Execute
            sut.StartRecurring();
            // Wait to be finsihed for maximum 10 sec.
            finishedEvent.Wait(TimeSpan.FromSeconds(10));

            // Assert
            Assert.AreEqual("Init", sut.Message, "Only Init succeded, because of exception");
        }

    }
}
