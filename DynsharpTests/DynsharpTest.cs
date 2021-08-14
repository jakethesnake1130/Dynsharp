using NUnit.Framework;
using Dynsharp;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace DynsharpTests
{
    public class Tests
    {
        string linuxOutput = "";
        string windowsOutput = "";

        [SetUp]
        public void Setup()
        {
            using (var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("DynsharpTests.LinuxOutput.txt"))
            using (var reader = new StreamReader(stream))
            {
                linuxOutput = reader.ReadToEnd();
            }

            using (var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("DynsharpTests.windowsOutput.txt"))
            using (var reader = new StreamReader(stream))
            {

                windowsOutput = reader.ReadToEnd();
            }
        }

        [Test]
        public void GrabOutput()
        {
            Assert.IsNotNull(linuxOutput);
            Assert.IsNotNull(windowsOutput);
        }

        [Test]
        public void WindowsProperParse()
       {
          bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

          if (!isWindows)
          {
            Assert.Ignore("Skipping on a non-Windows operating system");
          }

            List<string> winParsed = ParseIp.WindowsParse(windowsOutput);
            string test1 = "\n";
            string test2 = "";

            foreach (var item in winParsed)
            {
                Assert.AreNotEqual(test1, item);
                Assert.AreNotEqual(test2, item);            
            }
            Assert.AreEqual(winParsed.Count, 33);
        }

        [Test]
        public void LinuxProperParse()
        {
          bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

          if (!isLinux)
          {
            Assert.Ignore("Skipping on a non-Linux operating system");
          }
            List<string> linParsed = ParseIp.LinuxParse(linuxOutput);
            string test1 = "\n";
            string test2 = "";

            foreach (var item in linParsed)
            {
                Assert.AreNotEqual(test1, item);
                Assert.AreNotEqual(test2, item);
            }
            Assert.AreEqual(linParsed.Count, 5);
        }

        [Test]
        public void WindowsProperReturn()
        {
          bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

          if (!isWindows)
          {
            Assert.Ignore("Skipping on a non-Windows operating system");
          }
            List<string> winReturn = ReturnIp.WindowsReturn(ParseIp.WindowsParse(windowsOutput));
            Regex testIp = new(@"\d+\.\d+\.\d+\.\d+");

            foreach (var item in winReturn)
            {
                Assert.IsTrue(testIp.IsMatch(item));
            }
            Assert.AreEqual(3, winReturn.Count);
        }

        [Test]
        public void LinuxProperReturn()
        {
          bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

          if (!isLinux)
          {
            Assert.Ignore("Skipping on a non-Linux operating system");
          }

            List<string> linReturn = ReturnIp.LinuxReturn(ParseIp.LinuxParse(linuxOutput));
            Regex testIp = new(@"\d+\.\d+\.\d+\.\d+");

            foreach (var item in linReturn)
            {
                Assert.IsTrue(testIp.IsMatch(item));
            }
            Assert.AreEqual(4, linReturn.Count);
        }
    }
}
