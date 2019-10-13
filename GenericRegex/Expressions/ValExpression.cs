using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class ValExpression<T> : ExpressionBase<T>
    {
        public ValExpression(T value)
        {
            Value = value;
        }

        public T Value { get; }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (!context.IsEndOfSequence && EqualityComparer<T>.Default.Equals(context.CurrentItem, Value))
            {
                yield return context.WithIndex(context.Index + 1);
            }
        }

        public override string ToString() => $"Val({Value?.ToString()})";
    }
}
