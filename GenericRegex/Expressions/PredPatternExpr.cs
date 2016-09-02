using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class PredPatternExpr<T> : PatternExpr<T>
    {
        public Func<T, bool> Predicate { get; }

        public PredPatternExpr(Func<T, bool> predicate)
        {
            Predicate = predicate;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (!context.IsEndOfSequence && Predicate(context.CurrentItem))
            {
                yield return context.WithIndex(context.Index + 1);
            }
        }

        public override string ToString() => $"Pred({Predicate})";
    }
}
