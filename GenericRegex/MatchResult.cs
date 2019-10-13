using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class MatchResult<T>
    {
        internal MatchResult(MatchContext<T> context, int startIndex)
        {
            this._context = context;
            StartIndex = startIndex;
            Elements = context.GetSubsequence(StartIndex, Length).ToList();
            Groups = new GroupContainer<T>(context);
        }

        public int StartIndex { get; }

        public int EndIndex => _context.Index;

        public int Length => EndIndex - StartIndex;

        public IReadOnlyList<T> Elements { get; }

        public GroupContainer<T> Groups { get; }

        readonly MatchContext<T> _context;
    }
}
