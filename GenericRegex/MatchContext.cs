using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class MatchContext<T>
    {
        public MatchContext(IndexedSequence<T> inputSequence)
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

        IndexedSequence<T> InputSequence { get; }

        public int Index { get; }

        public ImmutableDictionary<int, MatchReference> MatchReferences { get; }

        public MatchContext<T> WithIndex(int index) => new MatchContext<T>(this, index);

        public MatchContext<T> WithGroupId(int id, MatchReference matchReference) => new MatchContext<T>(this, id, matchReference);

        public bool IsEndOfSequence
        {
            get
            {
                return !InputSequence.TryGetElementAt(Index, out _);
            }
        }

        public T CurrentItem
        {
            get
            {
                if (!InputSequence.TryGetElementAt(Index, out T element))
                {
                    throw new InvalidOperationException();
                }

                return element;
            }
        }

        public IEnumerable<T> GetSubsequence(int startIndex, int length)
        {
            return InputSequence.GetSubsequence(startIndex, length);
        }

        public override string ToString() => $"Index = {Index}, InputSequence = {string.Join(", ", InputSequence.GetSubsequence(0, Index))}";
    }
}
