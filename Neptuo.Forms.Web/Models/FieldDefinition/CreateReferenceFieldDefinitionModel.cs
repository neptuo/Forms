using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neptuo.Forms.Web.Models
{
    public class CreateReferenceFieldDefinitionModel : EditFieldDefinitionModel
    {
        [Display(Name = "Target field definition")]
        [Required(ErrorMessage = "Target field definition is required!")]
        public int TargetFieldDefinitionID { get; set; }

        public IDictionary<string, IEnumerable<LightFieldDefinition>> Fields { get; set; }

        public IDictionary<string, IEnumerable<SelectListItem>> GetFields()
        {
            IDictionary<string, IEnumerable<SelectListItem>> result = new Dictionary<string, IEnumerable<SelectListItem>>();
            foreach (KeyValuePair<string, IEnumerable<LightFieldDefinition>> item in Fields)
            {
                result.Add(item.Key, item.Value.Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.ID.ToString()
                }));
            }
            return result;
        }
    }

    public class LightFormDefinition
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }

    public class LightFieldDefinition
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}