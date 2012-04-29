using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neptuo.Forms.Web.Models
{
    public class ListProjectModel
    {
        public int ProjectID { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public bool IsOwner { get; set; }
    }
}