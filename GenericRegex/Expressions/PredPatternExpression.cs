using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class PredExpression<T> : ExpressionBase<T>
    {
        public PredExpression(Func<T, bool> predicate)
        {
            Predicate = predicate;
        }

        public Func<T, bool> Predicate { get; }

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
