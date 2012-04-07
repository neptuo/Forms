using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    public class RemoteCredentials
    {
        [Key]
        [ForeignKey("UserAccount")]
        public int UserAccountID { get; set; }

        public UserAccount UserAccount { get; set; }

        public string Username { get; set; }
    }
}
