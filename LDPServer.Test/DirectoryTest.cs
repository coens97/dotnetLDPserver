using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LDPServer.Business;
using LDPServer.Common.DTO;
using LDPServer.Data;
using LDPServer.Common.Interfaces;
using Moq;

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
            var inputRescources = new ResourcesDirectory
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
            var inputRescources = new ResourcesDirectory
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

        [TestMethod]
        public void TestListOfFilesAndFolder()
        {
            // Use mock to not use the AppData directory instead a test folder
            var mockDataFolder = new Mock<IDataFolder>();
            mockDataFolder.Setup(x => x.GetDataFolder())
                .Returns("Testvectors/");

            var repository = new ResourceFileRepository(mockDataFolder.Object);
            var result = repository.GetRescourcesOfDirectory("");

            Assert.IsTrue(result.Exists, "Couldn't find test directory");
            Assert.IsTrue(result.RootDirectory.IsDirectory, "Root directory should be directory");

            var filesAndFolders = result.Rescources.ToList(); // Prevent using iterating over it multiple times

            Assert.IsTrue(filesAndFolders.Any(), "Directory should not be empty");

            Assert.IsTrue(filesAndFolders.Any(x => x.Name == "myfolder"), 
                "Root folder should contain empty folder");

            Assert.IsTrue(filesAndFolders.First(x => x.Name == "myfolder").IsDirectory,
                "Empty folder should be a directory");

            var filename = "directorylist.ttl";
            Assert.IsTrue(filesAndFolders.Any(x => x.Name == filename),
                "Root folder should contain directorylist.ttl");

            var file = filesAndFolders.First(x => x.Name == filename);
            Assert.IsFalse(file.IsDirectory,
                "File is not directory");

            Assert.AreEqual(filename, file.Name,
                "File is not directory");

            // Test in subdirectory
            var subresult = repository.GetRescourcesOfDirectory("myfolder/");
            Assert.IsTrue(subresult.Exists, "Couldn't find sub directory");
            Assert.IsTrue(subresult.RootDirectory.IsDirectory, "Sub directory should be directory");

            Assert.IsTrue(subresult.Rescources.Any(x => x.Name == "emptytextfile.txt"),
                "Subfolder should contain emptytextfile");
        }
    }
}