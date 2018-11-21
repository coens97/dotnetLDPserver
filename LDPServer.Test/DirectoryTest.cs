using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LDPServer.Business;

namespace LDPServer.Test
{
    [TestClass]
    public class DirectoryTest
    {
        [TestMethod]
        public void TestFoldersResult()
        {
            var rdfdirectory = new RdfDirectory();
            string expectedResult = File.ReadAllText("Testvectors/directorylist.ttl");

            string result = rdfdirectory.GetDirectory("/");
            Assert.AreEqual(expectedResult, result);
        }
    }
}
