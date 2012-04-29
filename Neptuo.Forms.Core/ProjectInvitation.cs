using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core
{
    /// <summary>
    /// Invitation to project.
    /// </summary>
    public class ProjectInvitation : BaseObject
    {
        public DateTime Created { get; set; }

        public int TargetUserID { get; set; }
        public virtual UserAccount TargetUser { get; set; }

        public int TargetProjectID { get; set; }
        public virtual Project TargetProject { get; set; }

        public int OwnerUserID { get; set; }
        public virtual UserAccount OwnerUser { get; set; }

        public int Type { get; set; }
    }

    /// <summary>
    /// Target user role in project.
    /// </summary>
    public static class ProjectInvitationType
    {
        /// <summary>
        /// Can only read definitions/data.
        /// </summary>
        public const int Reader = 1;

        /// <summary>
        /// Can read/create/update/delete defintinions.
        /// </summary>
        public const int Manager = 2;

        /// <summary>
        /// Transfers project ownership.
        /// </summary>
        public const int Owner = 3;

        /// <summary>
        /// Returns map of ID(storage value of project invitation type)/Name(Constant name).
        /// </summary>
        /// <returns>Map of ID(storage value of project invitation type)/Name(Constant name).</returns>
        public static IEnumerable<KeyValuePair<int, string>> GetTypes()
        {
            yield return new KeyValuePair<int, string>(Reader, "Reader");
            yield return new KeyValuePair<int, string>(Manager, "Manager");
            yield return new KeyValuePair<int, string>(Owner, "Owner");
        }
    }
}
