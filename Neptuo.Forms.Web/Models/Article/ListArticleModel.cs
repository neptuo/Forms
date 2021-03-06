﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Web.Mvc.Models;

namespace Neptuo.Forms.Web.Models
{
    public class ListArticleModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Culture { get; set; }

        public DateTime Modified { get; set; }

        public string AuthorFullname { get; set; }
    }
}