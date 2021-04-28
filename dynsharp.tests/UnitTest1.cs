using NUnit.Framework;
using IP_Address_Check;

namespace dynsharp.tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            // 1. grab linux/windows output


        }

        [Test]
        public void Test1()
        {
            var result = "";
            // 2. parse output
            // 3. extract ip
            // 4. test for correctness
            Assert.AreEqual(result, "10.0.1.20");
        }

        [Test]
        public void Test2()
        {
            var testContent = "10.0.1.20";

            Assert.IsFalse(ReturnIp.LacksIP(testContent));
            Assert.That(testContent != "dfadsfadsf");
        }
    }
}