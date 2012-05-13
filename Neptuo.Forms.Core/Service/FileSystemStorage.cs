using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Base file system file storage.
    /// </summary>
    public class FileSystemStorage : IFileStorage
    {
        public static string StoragePath = null;

        public byte[] GetData(string filename)
        {
            if (StoragePath == null)
                throw new Exception("StoragePath is not set!");

            string path = Path.Combine(StoragePath, filename);
            if (File.Exists(path))
                return File.ReadAllBytes(path);

            return null;
        }

        public string InsertData(byte[] data)
        {
            if (StoragePath == null)
                throw new Exception("StoragePath is not set!");

            string filename = Guid.NewGuid().ToString();
            File.WriteAllBytes(Path.Combine(StoragePath, filename), data);
            return filename;
        }
    }
}
