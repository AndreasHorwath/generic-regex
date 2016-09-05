using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class MatchContext<T>
    {
        public int Index { get; }
        public ImmutableDictionary<int, MatchReference> MatchReferences { get; }

        IndexedSequence<T> InputSequence { get; }

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

        public MatchContext<T> WithIndex(int index) => new MatchContext<T>(this, index);
        public MatchContext<T> WithGroupId(int id, MatchReference matchReference) => new MatchContext<T>(this, id, matchReference);

        public bool IsEndOfSequence
        {
            get
            {
                T element;
                return !InputSequence.TryGetElementAt(Index, out element);
            }
        }

        public T CurrentItem
        {
            get
            {
                T element;
                if (!InputSequence.TryGetElementAt(Index, out element))
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
