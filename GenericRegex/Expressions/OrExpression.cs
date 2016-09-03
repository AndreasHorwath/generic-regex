using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRegex
{
    class OrExpression<T> : ExpressionBase<T>
    {
        readonly List<ExpressionBase<T>> expressions;

        public OrExpression(IEnumerable<ExpressionBase<T>> expressions)
        {
            this.expressions = expressions.ToList();
        }

        public IReadOnlyCollection<ExpressionBase<T>> Expressions => expressions;

        internal override IEnumerable<MatchContext<T>> Match(MatchContext<T> context)
        {
            return expressions.SelectMany(e => e.Match(context));
        }

        public override string ToString() => $"Or({string.Join(", ", expressions)})";
    }
}
