using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPServer.Common.DTO
{
    public class UploadFile
    {
        public string FileName { get; set; }
        public Action<string> SaveAs { get; set; }
    }
}
