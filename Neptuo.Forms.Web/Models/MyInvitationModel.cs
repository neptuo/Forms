using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neptuo.Forms.Core;

namespace Neptuo.Forms.Web.Models
{
    public class MyInvitationModel
    {
        public int ID { get; set; }

        public string ProjectName { get; set; }

        public string OwnerFullname { get; set; }

        public DateTime Created { get; set; }

        public int Type { get; set; }

        public string TypeName { get { return ProjectInvitationType.GetTypes().First(t => t.Key == Type).Value; } }
    }
}