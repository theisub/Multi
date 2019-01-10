
namespace Tests
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Core;

    [TestFixture]
    public class TestsSolution2AsyncAwait
    {
        /// <summary>
        /// Synchronizes the test with code via asynchronous await.
        /// Test needs to be executed with NUnit Runner 2.6.2 or later.
        /// </summary>
        [Test]
        public async Task SynchronizeTestWithCodeViaAwait()
        {
            // Setup
            var sut = new SystemUnderTestAsyncAwait();

            // Schedule operation to run asynchronously and wait until it is finished.
            await sut.DoWorkAsync();

            // Assert
            Assert.AreEqual("Init Work", sut.Message);
        }


        /// <summary>
        /// Synchronizes the test with code via returned task. Use this in case
        /// the test runner cannot handle "public async Task" test method signatures.
        /// </summary>
        [Test]
        public void SynchronizeTestWithCodeViaReturnedTask()
        {
            // Setup
            var sut = new SystemUnderTestAsyncAwait();

            // Schedule operation to run asynchronously and wait until it is finished.
            Task task = sut.DoWorkAsync();
            task.Wait();

            // Assert
            Assert.AreEqual("Init Work", sut.Message);
        }


        /// <summary>
        /// Synchronizes the test with code via asynchronous await.
        /// The test expects that the method throws an exception.
        /// Test needs to be executed with NUnit Runner 2.6.2 or later.
        /// </summary>
        [Test]
        public async Task SynchronizeTestWithCodeViaAsyncAwaitAndExpectException()
        {
            // Setup
            var sut = new SystemUnderTestAsyncAwait();
            sut.PrepareToThrowException();

            // Execute
            await sut.DoWorkAsync();
        }

        [Test]
        public async Task SynchronizeTestWithRecurringOperationViaAwait()
        {
            // Setup
            var sut = new SystemUnderTestAsyncAwait();

            // Execute code to set up timer with 1 sec delay and interval.
            var firstNotification = sut.StartRecurring();
            
            // Wait that operation has finished two times
            var secondNotification = await firstNotification.GetNext();
            await secondNotification.GetNext();
            sut.StopRecurring();

            // Assert
            Assert.AreEqual("Init Poll Poll", sut.Message);
        }
    }
}
