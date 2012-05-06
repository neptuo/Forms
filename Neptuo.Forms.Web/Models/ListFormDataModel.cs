using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neptuo.Web.Mvc.Models;
using Neptuo.Forms.Core;

namespace Neptuo.Forms.Web.Models
{
    public class ListFormDataModel
    {
        public int ProjectID { get; set; }

        public string FormName { get; set; }

        public IEnumerable<SimpleColumn> Columns { get; set; }

        public IEnumerable<ListItemFormDataModel> Items { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }

    public class SimpleColumn
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }

    public class ListItemFormDataModel
    {
        public int ID { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<FieldData> Columns { get; set; }
    }
}