﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neptuo.Forms.Web.Models
{
    public class ListFieldDefinitionModel
    {
        public int ProjectID { get; set; }

        public IEnumerable<ListItemFieldDefinitionModel> Fields { get; set; }
    }

    public class ListItemFieldDefinitionModel
    {
        public int FieldDefinitionID { get; set; }

        public string Name { get; set; }

        public int FieldType { get; set; }

        public bool Required { get; set; }
    }
}