using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class PredWithBackrefsExpressions<T> : ExpressionBase<T>
    {
        public Func<T, CapturingGroupContainer<T>, bool> Predicate { get; }

        public PredWithBackrefsExpressions(Func<T, CapturingGroupContainer<T>, bool> predicate)
        {
            Predicate = predicate;
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            var groups =  new CapturingGroupContainer<T>(context);

            if (!context.IsEndOfSequence && Predicate(context.CurrentItem, groups))
            {
                yield return context.WithIndex(context.Index + 1);
            }
        }

        public override string ToString() => $"Pred({Predicate})";
    }
}
