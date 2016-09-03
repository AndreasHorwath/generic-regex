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
            if (context != null)
            {
                groups = context.MatchReferences.ToDictionary(e => e.Key, e =>
                {
                    MatchReference mr = e.Value;
                    return new Match<T>(mr.StartIndex, mr.Length, context.InputSequence.CurrentList.Skip(mr.StartIndex).Take(mr.Length).ToList());
                });
            }
            else
            {
                groups = new Dictionary<int, Match<T>>();
            }
        }

        public Match<T> this[int groupId]
        {
            get
            {
                Match<T> match;
                groups.TryGetValue(groupId, out match);
                return match;
            }
        }

        public int Count => groups.Count;

        public ICollection<int> Keys => groups.Keys;

        public ICollection<Match<T>> Values => groups.Values;
    }
}
