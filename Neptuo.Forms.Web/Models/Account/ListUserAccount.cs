using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Neptuo.Forms.Web.Models
{
    public class ListUserAccount
    {
        public int ID { get; set; }

        public string PublicIdentifier { get; set; }

        public string Fullname { get; set; }

        public string LocalUsername { get; set; }

        public string RemoteUsername { get; set; }

        public bool Enabled { get; set; }

        public DateTime Created { get; set; }

        public string UserRole { get; set; }

        public string GetUsername()
        {
            if (!String.IsNullOrEmpty(LocalUsername))
                return LocalUsername;

            return RemoteUsername;
        }
    }
}