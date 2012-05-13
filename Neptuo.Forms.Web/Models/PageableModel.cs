using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Web.Mvc.Models;

namespace Neptuo.Forms.Web.Models
{
    public class PageableModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public PageableModel(IEnumerable<T> items, PagingInfo pagingInfo)
        {
            Items = items;
            PagingInfo = pagingInfo;
        }
    }
}