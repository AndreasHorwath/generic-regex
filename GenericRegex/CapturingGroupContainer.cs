using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class CapturingGroupContainer<T>
    {
        readonly Dictionary<int, Match<T>> groups;

        internal CapturingGroupContainer(MatchContext<T> context)
        {
            groups = context.MatchReferences.ToDictionary(e => e.Key, e =>
            {
                MatchReference mr = e.Value;
                return new Match<T>(mr.StartIndex, mr.Length, context.GetSubsequence(mr.StartIndex, mr.Length));
            });
        }

        public Match<T> this[int expressionId]
        {
            get
            {
                Match<T> match;
                groups.TryGetValue(expressionId, out match);
                return match;
            }
        }

        public int Count => groups.Count;

        public ICollection<int> Keys => groups.Keys;

        public ICollection<Match<T>> Values => groups.Values;
    }
}
