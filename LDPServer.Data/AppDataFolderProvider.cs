using LDPServer.Common.Interfaces;
using System;
using System.IO;

namespace LDPServer.Data
{
    /// <summary>
    /// Creates a working directory in %AppData%/LDPServer/
    /// </summary>
    public class AppDataFolderProvider : IDataFolder
    {
        // Class should be initialized as singleton
        private string _dataFolder = null;
        public string GetDataFolder()
        {
            if (_dataFolder == null) // If not initialized
            {

                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/LDPServer/";
                Directory.CreateDirectory(appDataPath);
                _dataFolder = appDataPath;
            }
            return _dataFolder;
        }
    }
}
