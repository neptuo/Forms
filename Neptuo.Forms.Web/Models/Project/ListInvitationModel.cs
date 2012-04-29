using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neptuo.Forms.Core;

namespace Neptuo.Forms.Web.Models
{
    public class ListInvitationModel
    {
        public IEnumerable<ListItemInvitationModel> Invitations { get; set; }

        public CreateInvitationModel CreateModel { get; set; }
    }

    public class ListItemInvitationModel
    {
        public int ID { get; set; }

        public string ProjectName { get; set; }

        public string OwnerFullname { get; set; }

        public string OwnerPublicIdentifier { get; set; }

        public string TargetUserFullname { get; set; }

        public string TargetUserPublicIdentifier { get; set; }

        public DateTime Created { get; set; }

        public int Type { get; set; }
    }

    public class CreateInvitationModel
    {
        [Display(Name="Target user public identifier")]
        [Required(ErrorMessage="User identifier is required!")]
        public string TargetUserPublicIdentifier { get; set; }

        [Display(Name="Invitation type")]
        [Required(ErrorMessage="Invitation type is required!")]
        public int Type { get; set; }

        public IEnumerable<SelectListItem> GetTypes()
        {
            foreach (KeyValuePair<int, string> item in ProjectInvitationType.GetTypes())
            {
                yield return new SelectListItem
                {
                    Text = item.Value,
                    Value = item.Key.ToString(),
                    Selected = item.Key == Type
                };
            }
        }
    }
}