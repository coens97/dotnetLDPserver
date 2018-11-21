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
                RootDirectory = new ResourceMetaData
                    {
                       LastModificationTime = 1542716708,
                       Size = 0,
                       IsDirectory = true,
                    },
                Rescources = new[] {
                    new ResourceMetaData
                    {
                       Name = "testfolder",
                       LastModificationTime = 1542716708,
                       Size = 0,
                       IsDirectory = true,
                    },
                    new ResourceMetaData
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

        [TestMethod]
        public void TestFilesResult()
        {
            // Service that will be tested
            var rdfService = new RdfService();

            // Text file with expected result
            string expectedResult = File.ReadAllText("Testvectors/fileslist.ttl");

            // Input data
            var inputRescources = new RescourcesDirectory
            {
                RootDirectory = new ResourceMetaData
                {
                    LastModificationTime = 1542716708,
                    Size = 0,
                    IsDirectory = true,
                },
                Rescources = new[] {
                    new ResourceMetaData
                    {
                       Name = "travel.pdf",
                       LastModificationTime = 1542716708,
                       Size = 299157,
                       IsDirectory = false,
                    },
                    new ResourceMetaData
                    {
                       Name = "picture.png",
                       LastModificationTime = 1542716758,
                       Size = 499157,
                       IsDirectory = false,
                    },
                     new ResourceMetaData
                    {
                       Name = "anotherpicture.png",
                       LastModificationTime = 1542716858,
                       Size = 299157,
                       IsDirectory = false,
                    },
                    new ResourceMetaData
                    {
                       Name = "noextension",
                       LastModificationTime = 1542716758,
                       Size = 899157,
                       IsDirectory = false,
                    }
                }
            };

            string result = rdfService.RescourcesToText("https://localhost/", inputRescources);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
