using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Service
{
    /// <summary>
    /// Represents storage for file data.
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>
        /// Returns data associated with <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <returns>File data.</returns>
        byte[] GetData(string filename);

        /// <summary>
        /// Inserts new file to storage.
        /// </summary>
        /// <param name="data">File data.</param>
        /// <returns>Associated filename.</returns>
        string InsertData(byte[] data);
    }
}
