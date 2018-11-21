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
            var p2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
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
    }
}
