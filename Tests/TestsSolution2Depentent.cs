namespace Tests
{
    using Core;
    using NUnit.Framework;
    using Rhino.Mocks;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class TestsSolution2Depentent
    {
        [Test]
        public void SynchronizeTestWithCodeViaDependentObjectAndThreadSynchonizationEvent()
        {
            // Setup
            var dependentStub = MockRepository.GenerateStub<ISomeInterface>();
            var sut = new SystemUnderTestDependent(dependentStub);
            var finishedEvent = new ManualResetEventSlim();
            dependentStub.Stub(x => x.DoMore()).
                WhenCalled(x => finishedEvent.Set());

            // Schedule operation to run asynchronously.
            sut.StartAsynchronousOperation();

            // Wait for operation to be finished for maximum 2 sec.
            finishedEvent.Wait(TimeSpan.FromSeconds(2));

            // Assert outcome of operation
            Assert.AreEqual("Init Work", sut.Message);
        }

        [Test]
        public async void SynchronizeTestWithCodeViaDependentObjectAndTaskCompletionSource()
        {
            // Setup
            var dependentStub = MockRepository.GenerateStub<ISomeInterface>();
            var sut = new SystemUnderTestDependent(dependentStub);
            var completionSource = new TaskCompletionSource<bool>();
            dependentStub.Stub(x => x.DoMore()).
                WhenCalled(x => completionSource.SetResult(true));

            // Schedule operation to run asynchronously.
            sut.StartAsynchronousOperation();

            // Wait for operation to be finished for maximum 2 sec.
            await completionSource.Task;

            // Assert outcome of operation
            Assert.AreEqual("Init Work", sut.Message);
        }


        [Test]
        public void SynchronizeTestWithCodeViaDependentObjectAndExceptionIsThrown()
        {
            // Setup
            
            var dependentStub = MockRepository.GenerateStub<ISomeInterface>();
            var sut = new SystemUnderTestDependent(dependentStub);
            sut.PrepareToThrowException();
            var finishedEvent = new ManualResetEventSlim();
            dependentStub.Stub(x => x.DoMore()).
                WhenCalled(x => finishedEvent.Set());

            // Execute
            sut.StartAsynchronousOperation();
            // Wait to be finsihed for maximum 2 sec.
            finishedEvent.Wait(TimeSpan.FromSeconds(2));

            // Assert (we cannot detect exception here because it is thrown in another thread)
            // Although the execution is aborted immediately, the test always takes 10 seconds.
            Assert.AreEqual("Init", sut.Message,"Only Init is done, work is aborted due to exception");
        }

        [Test]
        public void SynchronizeTestWithTimerViaDependentObject()
        {
            // Setup
            var dependentStub = MockRepository.GenerateStub<ISomeInterface>();
            var sut = new SystemUnderTestDependent(dependentStub);
            var finishedEvent = new ManualResetEventSlim();
            dependentStub.Stub(x => x.DoMore()).
                WhenCalled(x => finishedEvent.Set());

            // Execute
            sut.StartRecurring();
            // Wait to be finsihed for maximum 2 sec.
            finishedEvent.Wait(TimeSpan.FromSeconds(2));
            sut.StopRecurring();

            // Assert
            Assert.AreEqual("Init Poll", sut.Message);
        }

        [Test]
        public void SynchronizeTestWithTimerViaDependentObjectAndExpectException()
        {
            // Setup
            var dependentStub = MockRepository.GenerateStub<ISomeInterface>();
            var sut = new SystemUnderTestDependent(dependentStub);
            sut.PrepareToThrowException();
            var finishedEvent = new ManualResetEventSlim();
            dependentStub.Stub(x => x.DoMore()).
                WhenCalled(x => finishedEvent.Set());

            // Execute
            sut.StartRecurring();
            // Wait to be finsihed for maximum 2 sec.
            finishedEvent.Wait(TimeSpan.FromSeconds(2));

            // Assert
            Assert.AreEqual("Init", sut.Message, "Only Init succeded, because of exception");
        }

    }
}
