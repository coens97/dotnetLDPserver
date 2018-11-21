using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LDPServer.Business;
using LDPServer.Common.DTO;

namespace LDPServer.Test
{
    [TestClass]
    public class DirectoryTest
    {
        [TestMethod]
        public void TestFoldersResult()
        {
            // Service that will be tested
            var rdfService = new RdfService();

            // Text file with expected result
            string expectedResult = File.ReadAllText("Testvectors/directorylist.ttl");

            // Input data
            var inputRescources = new RescourcesDirectory
            {
                RootDirectory = new RescourceMetaData
                    {
                       LastModificationTime = 1542716708,
                       Size = 0,
                       IsDirectory = true,
                    },
                Rescources = new[] {
                    new RescourceMetaData
                    {
                       Name = "testfolder",
                       LastModificationTime = 1542716708,
                       Size = 0,
                       IsDirectory = true,
                    },
                    new RescourceMetaData
                    {
                       Name = "music",
                       LastModificationTime = 1542716758,
                       Size = 0,
                       IsDirectory = true,
                    }
                }
            };

            string result = rdfService.RescourcesToText("https://localhost/", inputRescources);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
