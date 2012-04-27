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

        public int ProjectInvitationType { get; set; }
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
        public const int Manager = 1;

        /// <summary>
        /// Returns map of ID(storage value of project invitation type)/Name(Constant name).
        /// </summary>
        /// <returns>Map of ID(storage value of project invitation type)/Name(Constant name).</returns>
        public static IEnumerable<KeyValuePair<int, string>> GetTypes()
        {
            yield return new KeyValuePair<int, string>(Reader, "Reader");
            yield return new KeyValuePair<int, string>(Reader, "Manager");
        }
    }
}
