
namespace Tests
{
    using System;
    using Core;
    using System.Threading.Tasks;
    using NUnit.Framework;

    /// <summary>
    /// Testing the Functionality class independent of the SystemUnderTestHumbleObject class.
    /// </summary>
    [TestFixture]
    public class TestsSolution1Functionality
    {
        private Functionality functionality;

        [SetUp]
        public void SetUp()
        {
            functionality = new Functionality();
        }

        [Test]
        public void TestInitFunctiality()
        {
            // Execute
            functionality.Init();

            // Assert
            Assert.AreEqual("Init", functionality.Message);
        }

        [Test]
        public void TestDo1Functiality()
        {
            // Execute
            functionality.Do1();

            // Assert
            Assert.AreEqual(" Work", functionality.Message);
        }

        [Test]
        public void TestDo1FunctialityWithExeception()
        {
            // Setup
            functionality.PrepareToThrowException();

            // Execute, Assert
            Assert.That(
                functionality.Do1,
                Throws.TypeOf<Exception>());
        }

        [Test]
        public void TestDo2FunctialityOk()
        {
            // Execute
            functionality.Do2(TaskStatus.RanToCompletion);

            // Assert
            Assert.AreEqual(" OK", functionality.Message);
        }

        [Test]
        public void TestDo2FunctialityNok()
        {
            // Execute
            functionality.Do2(TaskStatus.Faulted);

            // Assert
            Assert.AreEqual(" NOK", functionality.Message);
        }

        [Test]
        public void TestDoRecurringFunctiality()
        {
            // Execute
            functionality.DoRecurring();

            // Assert
            Assert.AreEqual(" Poll", functionality.Message);
        }

        [Test]
        public void TestDoRecurringFunctialityWithExeception()
        {
            // Setup
            functionality.PrepareToThrowException();

            // Execute, Assert
            Assert.That(
                functionality.DoRecurring,
                Throws.TypeOf<Exception>());
        }

    }
}
