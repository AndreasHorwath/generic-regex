using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class GroupContainer<T>
    {
        readonly MatchContext<T> context;

        internal GroupContainer(MatchContext<T> context)
        {
            this.context = context;
        }

        public Match<T> this[int expressionId]
        {
            get
            {
                MatchReference matchReference;
                if (context.MatchReferences.TryGetValue(expressionId, out matchReference))
                {
                    return new Match<T>(matchReference.StartIndex, matchReference.Length, context.GetSubsequence(matchReference.StartIndex, matchReference.Length));
                }

                return null;
            }
        }

        public int Count => context.MatchReferences.Count;

        public IEnumerable<int> Keys => context.MatchReferences.Keys;
    }
}
