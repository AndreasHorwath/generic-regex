using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class MatchResult<T>
    {
        public int StartIndex { get; }
        public int EndIndex => context.Index;
        public int Length => EndIndex - StartIndex;
        public IReadOnlyList<T> Elements { get; }
        public GroupContainer<T> Groups { get; }

        readonly MatchContext<T> context;

        internal MatchResult(MatchContext<T> context, int startIndex)
        {
            this.context = context;
            StartIndex = startIndex;
            Elements = context.GetSubsequence(StartIndex, Length).ToList();
            Groups = new GroupContainer<T>(context);
        }
    }
}
