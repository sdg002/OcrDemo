using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class SimpleMoqTests
    {
        [TestMethod]
        public void Method_MustBe_Overridable()
        {
            try
            {
                Moq.Mock<Demo> mockDemo = new Moq.Mock<Demo>();
                mockDemo.Setup(m => m.DoSomeWork()).Callback(this.MockedDoSomeWork);
                var obj = mockDemo.Object;
                obj.DoSomeWork();
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception ex)
            {

            }
        }
        public void MockedDoSomeWork()
        {
            Trace.WriteLine("Inside MockedDoSomeWork");
        }
        [TestMethod]
        public void Mock_Virtual_Method()
        {
            Moq.Mock<Demo> mockDemo = new Moq.Mock<Demo>();
            int counter = 0;
            mockDemo.Setup(m => m.DoSomeWork2()).Callback(()=> counter++);
            var obj = mockDemo.Object;
            obj.DoSomeWork2();
            Assert.AreEqual(1, counter);

        }
        /// <summary>
        /// This is an example which demonstrates the limitation of the Moq framework
        /// We are unable to mock the read only property Id of the Process class because this is not overridable
        /// Refer SFO
        /// https://stackoverflow.com/questions/1015315/how-do-you-mock-class-with-readonly-property
        /// </summary>
        [TestMethod]
        public void ReadOnlyProperty_Cannot_Be_Mocked_Unless_Overridable()
        {
            try
            {
                Moq.Mock<Process> mockProc = new Moq.Mock<Process>();
                mockProc.Setup(m => m.Id).Returns(999);
                mockProc.Setup(m => m.ExitCode).Returns(-1);
                var obj = mockProc.Object;
                Assert.Fail("Expected exception was not thrown");
            }
            catch ( Exception ex)
            {

            }
        }

    }
    public class Demo
    {
        public void DoSomeWork()
        {

        }
        virtual public void DoSomeWork2()
        {

        }
        private Process Launch(ProcessStartInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
