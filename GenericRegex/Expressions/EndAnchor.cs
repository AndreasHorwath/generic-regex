using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class EndAnchor<T> : ExpressionBase<T>
    {
        public static EndAnchor<T> Instance { get; } = new EndAnchor<T>();

        public EndAnchor()
        {
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (context.IsEndOfSequence)
            {
                yield return context;
            }
        }

        public override string ToString() => "EndAnchor";
    }
}
