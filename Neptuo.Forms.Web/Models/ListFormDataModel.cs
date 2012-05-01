using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Web.Mvc.Models;

namespace Neptuo.Forms.Web.Models
{
    public class ListFormDataModel
    {
        public IEnumerable<ListItemFormDataModel> Items { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }

    public class ListItemFormDataModel
    {
        public int ID { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<FieldDataModel> Columns { get; set; }
    }

    public class FieldDataModel
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}