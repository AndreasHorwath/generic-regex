using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    public class GroupContainer<T>
    {
        internal GroupContainer(MatchContext<T> context)
        {
            _context = context;
        }

        readonly MatchContext<T> _context;

        public Match<T> this[int expressionId]
            => FindMatch(expressionId) ?? throw new ArgumentException($"No match for expressionId = {expressionId}", nameof(expressionId));

        public Match<T>? FindMatch(int expressionId)
        {
            if (_context.MatchReferences.TryGetValue(expressionId, out MatchReference matchReference))
            {
                return new Match<T>(matchReference.StartIndex, matchReference.Length, _context.GetSubsequence(matchReference.StartIndex, matchReference.Length));
            }

            return null;
        }

        public int Count => _context.MatchReferences.Count;

        public IEnumerable<int> Keys => _context.MatchReferences.Keys;
    }
}
