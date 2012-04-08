using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    public class Article : BaseObject
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Culture { get; set; }

        public DateTime Modified { get; set; }

        [ForeignKey("Author")]
        public int? AuthorUserID { get; set; }

        public virtual UserAccount Author { get; set; }
    }
}
