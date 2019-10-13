using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class PredWithBackrefsExpressions<T> : ExpressionBase<T>
    {
        public PredWithBackrefsExpressions(Func<T, GroupContainer<T>, bool> predicate)
        {
            Predicate = predicate;
        }

        public Func<T, GroupContainer<T>, bool> Predicate { get; }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            var groups =  new GroupContainer<T>(context);

            if (!context.IsEndOfSequence && Predicate(context.CurrentItem, groups))
            {
                yield return context.WithIndex(context.Index + 1);
            }
        }

        public override string ToString() => $"Pred({Predicate})";
    }
}
