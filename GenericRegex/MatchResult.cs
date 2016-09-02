using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class MatchResult<T>
    {
        public bool Success { get; }
        public CapturingGroupContainer<T> Groups { get; }

        internal MatchResult(MatchContext<T> context)
        {
            Success = context != null;
            Groups = new CapturingGroupContainer<T>(context);
        }
    }

}
