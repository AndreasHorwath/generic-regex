using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class AnyElement<T> : ExpressionBase<T>
    {
        public static AnyElement<T> Instance { get; } = new AnyElement<T>();

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (!context.IsEndOfSequence)
            {
                yield return context.WithIndex(context.Index + 1);
            }
        }

        public override string ToString() => "AnyElement";
    }
}
