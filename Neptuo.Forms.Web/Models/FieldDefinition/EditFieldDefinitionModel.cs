using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Neptuo.Forms.Web.Models
{
    public class EditFieldDefinitionModel
    {
        [HiddenInput(DisplayValue = false)]
        public int FieldDefinitionID { get; set; }

        [Display(Name = "Field name")]
        [Required(ErrorMessage = "Field name is required!")]
        public string Name { get; set; }

        [Display(Name = "Field type")]
        [Required(ErrorMessage = "Field type is required!")]
        public int FieldType { get; set; }

        [Display(Name = "Is required")]
        public bool Required { get; set; }

        public int FormType { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int FormDefinitionID { get; set; }

        public IEnumerable<SelectListItem> GetFieldTypes()
        {
            foreach (KeyValuePair<int, string> item in Core.FieldType.GetTypes())
            {
                if (item.Key != Core.FieldType.ReferenceField)
                {
                    yield return new SelectListItem
                    {
                        Text = (L)item.Value,
                        Value = item.Key.ToString(),
                        Selected = item.Key == FieldType
                    };
                }
            }
        }

        public bool IsNew()
        {
            return FieldDefinitionID == 0;
        }
    }
}