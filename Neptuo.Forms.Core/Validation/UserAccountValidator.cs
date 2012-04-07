using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Neptuo.Web.DataAccess;
using Neptuo.Web.Validation;

namespace Neptuo.Forms.Core.Validation
{
    public class UserAccountValidator : GenericValidator<UserAccount>
    {
        [Dependency]
        IRepository<UserAccount> Users { get; set; }

        public override IList<ValidationResult> Validate(UserAccount entity)
        {
            

            return base.Validate(entity);
        }
    }
}
