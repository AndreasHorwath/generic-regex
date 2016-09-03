using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class StartAnchor<T> : ExpressionBase<T>
    {
        public static StartAnchor<T> Instance { get; } = new StartAnchor<T>();

        public StartAnchor()
        {
        }

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            if (context.Index == 0)
            {
                yield return context;
            }
        }

        public override string ToString() => "StartAnchor";
    }
}
