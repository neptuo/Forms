using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.DataAccess;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Base for domain entity.
    /// </summary>
    public class BaseObject : IBaseObject<int>
    {
        public int ID { get; set; }
    }
}
