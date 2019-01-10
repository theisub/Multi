namespace Tests
{
    using System.Security.Permissions;
    using System.Threading;
    using System.Windows.Threading;

    using Core;

    using NUnit.Framework;

    [TestFixture]
    public class TestsInitial
    {
        [Test]
        public void FragileAndSlowTest()
        {
            // Setup
            var sut = new SystemUnderTestInitial();

            // Operation to be tested will run in another thread.
            sut.StartAsynchronousOperation();

            // Bad Idea: Hoping that the other thread finishes execution after 2 seconds.
            Thread.Sleep(2000);

            // Assert outcome of the thread.
            Assert.AreEqual("Init Work", sut.Message);
        }

        [Test]
        public void FragileAndSlowTestWithSystemThreadingTimer()
        {
            // Setup
            var sut = new SystemUnderTestInitial();

            // Execute code to set up timer with 1 sec delay and interval:
            sut.StartRecurring();
            // Bad idea: Wait for timer to trigger three times.
            Thread.Sleep(3100);
            sut.StopRecurring();

            // Assert outcome
            Assert.AreEqual("Init Poll Poll Poll", sut.Message);
        }

        [Test]
        public void FragileAndSlowTestWithDispatcherTimer()
        {
            // Setup
            var sut = new SystemUnderTestInitialWithDispatcherTimer();

            // Execute code to set up timer with 1 sec delay and interval:
            sut.StartRecurring();

            // Bad idea: Wait for timer to trigger three times while 
            // running the WPF message pump with DispatcherUtil.DoEvents();
            int waitMilliseconds = 0;
            while (waitMilliseconds < 3500)
            {
                DispatcherUtil.DoEvents();
                Thread.Sleep(200);
                waitMilliseconds += 200;
            }
            sut.StopRecurring();

            // Assert outcome
            Assert.AreEqual("Init Poll Poll Poll", sut.Message);
        }

    }

    public static class DispatcherUtil
    {
        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
}
