namespace Tests
{
    using System;
    
    using System.Threading.Tasks;

    using Core;

    using NUnit.Framework;

    /// <summary>
    /// Testing the functionality of the SystemUnderTestHumbleLight via methods,
    /// which were formerly private. 
    /// </summary>
    [TestFixture]
    public class TestsSolution1HumbleLight
    {
        private SystemUnderTestHumbleLight sut;

        [SetUp]
        public void SetUp()
        {
            sut = new SystemUnderTestHumbleLight();
        }

        [Test]
        public void TestInitFunctiality()
        {
            // Execute
            sut.Init();

            // Assert
            Assert.AreEqual("Init", sut.Message);
        }

        [Test]
        public void TestDo1Functiality()
        {
            // Execute
            sut.Do1();

            // Assert
            Assert.AreEqual(" Work", sut.Message);
        }

        [Test]
        public void TestDo1FunctialityWithExeception()
        {
            // Setup
            sut.PrepareToThrowException();

            // Execute, Assert
            Assert.That(
                sut.Do1,
                Throws.TypeOf<Exception>());
        }

        [Test]
        public void TestDo2FunctialityOk()
        {
            // Execute
            sut.Do2(TaskStatus.RanToCompletion);

            // Assert
            Assert.AreEqual(" OK", sut.Message);
        }

        [Test]
        public void TestDo2FunctialityNok()
        {
            // Execute
            sut.Do2(TaskStatus.Faulted);

            // Assert
            Assert.AreEqual(" NOK", sut.Message);
        }

        [Test]
        public void TestDoRecurringFunctiality()
        {
            // Execute
            sut.DoRecurring();

            // Assert
            Assert.AreEqual(" Poll", sut.Message);
        }

        [Test]
        public void TestDoRecurringFunctialityWithExeception()
        {
            // Setup
            sut.PrepareToThrowException();

            // Execute, Assert
            Assert.That(
                sut.DoRecurring,
                Throws.TypeOf<Exception>());
        }

    }
}
