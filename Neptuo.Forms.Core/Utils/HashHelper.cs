using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Neptuo.Forms.Core.Utils
{
    public static class HashHelper
    {
        public static string ComputePublicIdentifier(string type, string name)
        {
            return Guid.NewGuid().ToString();
        }

        public static string ComputePassword(string username, string password)
        {
            return Sha1(username + password);
        }

        public static string Sha1(string text)
        {
            SHA1 hasher = SHA1.Create();
            return Encoding.UTF8.GetString(hasher.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }
    }
}
