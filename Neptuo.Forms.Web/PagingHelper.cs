using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Web.Mvc.Models;

namespace Neptuo.Forms.Web
{
    public class PagingHelper
    {
        public static PagingInfo CreateInfo<TDataItem>(IQueryable<TDataItem> dataSource, int pageIndex, int pageSize)
        {
            return new PagingInfo
            {
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize,
                TotalItems = dataSource.Count()
            };
        }

        public static IEnumerable<TDataItem> TakePage<TDataItem>(IQueryable<TDataItem> dataSource, int pageIndex, int pageSize)
        {
            return dataSource.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }

    public interface IPagingModel
    {
        PagingInfo PagingInfo { get; set; }
    }
}