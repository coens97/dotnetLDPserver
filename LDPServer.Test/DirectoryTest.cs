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
            var rdfdirectory = new RescourcesService();
            string expectedResult = File.ReadAllText("Testvectors/directorylist.ttl");

            string result = rdfdirectory.GetDirectoryRescources("/");
            Assert.AreEqual(expectedResult, result);
        }
    }
}
