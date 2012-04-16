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
            Random r = new Random((int)DateTime.Now.Ticks);
            return Guid.NewGuid().ToString().Substring(0, 13).Replace('-', name[r.Next(name.Length)]);
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
