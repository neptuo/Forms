using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    public class UserAccount : BaseObject
    {
        public string Fullname { get; set; }

        public string Email { get; set; }

        public bool Enabled { get; set; }

        public DateTime Created { get; set; }

        public string UserRole { get; set; }

        //[ForeignKey("LocalCredentials")]
        //public int? LocalCredentialsID { get; set; }

        public virtual LocalCredentials LocalCredentials { get; set; }

        //[ForeignKey("RemoteCredentials")]
        //public int? RemoteCredentialsID { get; set; }

        public virtual RemoteCredentials RemoteCredentials { get; set; }
    }
}
