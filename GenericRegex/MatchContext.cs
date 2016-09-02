using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class MatchContext<T>
    {
        public IReadOnlyList<T> InputSequence { get; }
        public int Index { get; }
        public ImmutableDictionary<int, MatchReference> MatchReferences { get; }

        public MatchContext(IReadOnlyList<T> inputSequence)
        {
            InputSequence = inputSequence;
            MatchReferences = ImmutableDictionary<int, MatchReference>.Empty;
        }

        MatchContext(MatchContext<T> context, int index)
        {
            InputSequence = context.InputSequence;
            Index = index;
            MatchReferences = context.MatchReferences;
        }

        MatchContext(MatchContext<T> context, int id, MatchReference matchReference)
        {
            InputSequence = context.InputSequence;
            Index = context.Index;
            MatchReferences = context.MatchReferences.Add(id, matchReference);
        }

        public MatchContext<T> WithIndex(int index) => new MatchContext<T>(this, index);
        public MatchContext<T> WithGroupId(int id, MatchReference matchReference) => new MatchContext<T>(this, id, matchReference);

        public bool IsEndOfSequence => Index >= InputSequence.Count;
        public T CurrentItem => InputSequence[Index];

        public override string ToString() => $"Index = {Index}, InputSequence = {string.Join(", ", InputSequence.ToArray())}";
    }
}
