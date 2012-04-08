using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Forms.Core.Validation
{
    [Serializable]
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException() { }
        public PermissionDeniedException(string message) : base(message) { }
        public PermissionDeniedException(string message, Exception inner) : base(message, inner) { }
        protected PermissionDeniedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
