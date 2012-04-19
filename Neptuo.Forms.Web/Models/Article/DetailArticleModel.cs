using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Web.Mvc.Models;

namespace Neptuo.Forms.Web.Models
{
    public class ListDetailArticleModel : QuickListArticleModel
    {
        public PagingInfo PagingInfo { get; set; }

        public ListDetailArticleModel(IEnumerable<DetailArticleModel> items, PagingInfo pagingInfo)
            : base(items)
        {
            PagingInfo = pagingInfo;
        }
    }

    public class QuickListArticleModel
    {
        public IEnumerable<DetailArticleModel> Items { get; set; }

        public QuickListArticleModel(IEnumerable<DetailArticleModel> items)
        {
            Items = items;
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