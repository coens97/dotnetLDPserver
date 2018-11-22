using LDPServer.Common.DTO;
using LDPServer.Common.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LDPServer.Data
{
    public class ResourceFileRepository : IResourceRepository
    {
        private IDataFolder _dataFolder;

        public ResourceFileRepository(IDataFolder dataFolder)
        {
            _dataFolder = dataFolder;
        }

        private int ToEpochTime(DateTime time)
        {
            return (int)(time - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public ResourcesDirectory GetRescourcesOfDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(_dataFolder.GetDataFolder() + path);

            if (!di.Exists) // If folder doesn't exist
                return new ResourcesDirectory
                {
                    Exists = false
                };

            // Root directory metadata
            var rootDirectory = new ResourceMetaData
            {
                LastModificationTime = ToEpochTime(di.LastWriteTimeUtc),
                IsDirectory = true
            };

            // Sub-directories metadata
            var directories = di.GetDirectories().Select(x => new ResourceMetaData
            {
                Name = x.Name,
                LastModificationTime = ToEpochTime(x.LastWriteTimeUtc),
                Size = 0,
                IsDirectory = true
            });

            // Files metadata
            var files = di.GetFiles().Select(x => new ResourceMetaData
            {
                Name = x.Name,
                LastModificationTime = ToEpochTime(x.LastWriteTimeUtc),
                Size = (int)x.Length,
                IsDirectory = false
            });

            return new ResourcesDirectory
            {
                Exists = true,
                RootDirectory = rootDirectory,
                Rescources = directories.Concat(files)
            };
        }

        public string CreateDirectory(string path, string folderName)
        {
            var folderPath = _dataFolder.GetDataFolder() + path + folderName;

            if (File.Exists(folderPath) || Directory.Exists(folderPath))
            { // Folder already exists, make an unique name
                var newFolderName = Guid.NewGuid().ToString().Substring(0, 8) + "-" + folderName;
                var newFolderPath = _dataFolder.GetDataFolder() + path + newFolderName;
                Directory.CreateDirectory(newFolderPath);
                return newFolderName;
            }
            // Folder doesn't exist
            Directory.CreateDirectory(folderPath);
            return folderName;
        }

        public string CreateFile(string path, string fileName)
        {
            var filePath = _dataFolder.GetDataFolder() + path + fileName;

            if (File.Exists(filePath) || Directory.Exists(filePath))
                { // File already exists, make an unique name
                var newFileName = Guid.NewGuid().ToString().Substring(0, 8) + "-" + fileName;
                var newFilePath = _dataFolder.GetDataFolder() + path + newFileName;
                File.CreateText(newFilePath);
                return newFileName;
            }
            // File doesn't exist
            File.CreateText(filePath).Dispose(); // Directly close the file
            return fileName;
        }
    }
}
