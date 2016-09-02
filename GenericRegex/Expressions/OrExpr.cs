using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class OrExpr<T> : PatternExpr<T>
    {
        readonly List<PatternExpr<T>> expressions;

        public OrExpr(IEnumerable<PatternExpr<T>> expressions)
        {
            this.expressions = expressions.ToList();
        }

        public IReadOnlyCollection<PatternExpr<T>> Expressions => expressions;

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return expressions.SelectMany(e => e.Match(context));
        }

        public override string ToString() => $"Or({string.Join(", ", expressions)})";
    }
}
