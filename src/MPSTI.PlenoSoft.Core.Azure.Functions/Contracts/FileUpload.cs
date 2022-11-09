using System.Collections.Generic;
using System.IO;

namespace MPSTI.PlenoSoft.Core.Azure.Functions.Contracts
{
    public class FileUpload
    {
        public List<FileData> Files { get; set; }
    }

    public class FileData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }

        public Stream OpenReadStream() => new MemoryStream(Data);
    }
}