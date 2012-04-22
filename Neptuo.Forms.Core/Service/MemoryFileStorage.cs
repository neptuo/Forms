using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Forms.Core.Utils;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// TESTING implementation! Not for production :)
    /// </summary>
    public class MemoryFileStorage : IFileStorage
    {
        private static Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

        public byte[] GetData(string filename)
        {
            byte[] result;
            if (files.TryGetValue(filename, out result))
                return result;

            return null;
        }

        public string InsertData(byte[] data)
        {
            string filename = HashHelper.ComputePublicIdentifier(GetType().Name, "NewFile");
            files.Add(filename, data);
            return filename;
        }
    }
}
