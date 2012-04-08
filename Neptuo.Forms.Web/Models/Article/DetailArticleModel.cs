using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Web.Mvc.Models;

namespace Neptuo.Forms.Web.Models
{
    public class ListDetailArticleModel
    {
        public IEnumerable<DetailArticleModel> Items { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public ListDetailArticleModel(IEnumerable<DetailArticleModel> items, PagingInfo pagingInfo)
        {
            Items = items;
            PagingInfo = pagingInfo;
        }
    }

    public class DetailArticleModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Modified { get; set; }

        public string AuthorFullname { get; set; }
    }
}